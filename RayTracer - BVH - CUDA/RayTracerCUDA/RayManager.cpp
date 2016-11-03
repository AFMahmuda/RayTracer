#include "RayManager.h"
#include <thread>

RayManager::RayManager()
{
}

void RayManager::traceRay(Ray & ray, Container & bin)
{
	if (bin.isIntersecting(ray))
	{
		if (bin.geo != nullptr)
		{
			auto tempStart = std::make_shared<Point3>(ray.start);
			auto tempDir = std::make_shared<Vec3>(ray.direction);

			//transform ray according to each shapes transformation
			ray.transInv(bin.geo->getTrans());

			if (bin.geo->isIntersecting(ray))
			{
				ray.intersectWith = bin.geo;
			}

			//assign original value for start and direction by memory equivalent to Transform(geometry.Trans);
			ray.start = *tempStart;
			ray.direction = *tempDir;
		}
		else
		{

			//not parallelised
			traceRay(std::ref(ray), *bin.lChild);
			traceRay(std::ref(ray), *bin.rChild);
		}
	}

}

MyColor & RayManager::getColor(const Ray & ray, Scene & scene, int bounce) {
	if (bounce <= 0 || ray.intersectWith == nullptr)
	{
		if (ray.type == Ray::REFRACTION || ray.type == Ray::REFRACTION)
			return MyColor();
		return MyColor();
		//return scene.defColor;
	}

	else
	{
		std::vector<std::shared_ptr< Light>> effectiveLights = populateLights(ray, scene.lights, *scene.bin);
		MyColor color = calcColor(ray, effectiveLights, scene.getAtt());

		MyColor refl = getRefl(ray, scene, bounce - 1);
		MyColor refr = getRefr(ray, scene, bounce - 1);
		color += (refl + refr);
		return color;
	}
}

MyColor RayManager::getRefl(const Ray & ray, Scene & scene, int bounce) {
	float cosI = ray.intersectWith->getNormal(ray.getHitReal()) * ray.direction;
	Vec3 normal = ray.intersectWith->getNormal(ray.getHitReal());
	if (cosI < 0)
	{
		normal *= -1.f;
	}

	Vec3 newDir = ray.direction + (normal * 2.0f * -(normal * ray.direction));
	Ray reflectRay(ray.getHitMin(), newDir);
	reflectRay.type = Ray::REFLECTION;
	traceRay(reflectRay, *scene.bin);
	return ray.intersectWith->mat.specular * getColor(reflectRay, scene, bounce);
}

MyColor RayManager::getRefr(const Ray & ray, Scene & scene, int bounce) {
	//TODO : IMPLEMENT
	return MyColor();
}

MyColor RayManager::calcColor(const Ray & ray, std::vector<std::shared_ptr<Light>> effectiveLights, Attenuation & attenuation)
{
	MyColor result = ray.intersectWith->ambient + ray.intersectWith->mat.emmission;
	Vec3 normal = ray.intersectWith->getNormal(ray.getHitMin());
	float cosI = ray.intersectWith->getNormal(ray.getHitReal()) * ray.direction;
	if (cosI > 0)
	{
		normal *= -1.f;
	}

	for (int i = 0; i < effectiveLights.size(); i++)
	{
		std::shared_ptr< Light> light = effectiveLights[i];
		Vec3 pointToLight = light->getPointToLight(ray.getHitMin());
		Vec3 halfAngleToLight = Vec3(Vec3(ray.direction * -1.0f) + pointToLight).Normalize();

		Material material = ray.intersectWith->mat;

		float attenuationValue = light->getAttValue(ray.getHitMin(), attenuation);
		pointToLight = pointToLight.Normalize();
		result +=
			attenuationValue * (MyColor)(*(light->color)) *
			((material.diffuse * (pointToLight * normal)) +
			(material.specular * std::powf(halfAngleToLight * normal, material.shininess)));
	}
	return result;
}

std::vector<std::shared_ptr<Light>> RayManager::populateLights(const Ray & ray, std::vector<std::shared_ptr<Light>> allLights, Container & bvh)
{
	std::vector <std::shared_ptr<Light>> res;
	for (size_t i = 0; i < allLights.size(); i++)
	{
		if (allLights[i]->isEffective(ray.getHitMin(), bvh))
			res.push_back(allLights[i]);
	}
	return res;
}


RayManager::~RayManager()
{
}

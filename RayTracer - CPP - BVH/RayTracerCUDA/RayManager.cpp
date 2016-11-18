#include "RayManager.h"
#include <thread>

RayManager::RayManager()
{
}

void RayManager::traceRay(Ray & ray, std::shared_ptr<Container> bvh)
{
	std::vector<std::shared_ptr<Container>> bins;
	bins.push_back(bvh);
	while (bins.size() > 0)
	{
		std::shared_ptr<Container> currBin = bins.back();
		bins.pop_back();
		if (currBin->isIntersecting(ray))
		{

			if (currBin->geo != nullptr)
			{
				Vec3 tempStart = ray.start;
				Vec3 tempDir = ray.direction;

				//transform ray according to each shapes transformation
				ray.transInv(currBin->geo->getTrans());

				if (currBin->geo->isIntersecting(ray))
					ray.intersectWith = currBin->geo;

				ray.start = tempStart;
				ray.direction = tempDir;
			}
			else
			{
				bins.push_back(currBin->rChild);
				bins.push_back(currBin->lChild);
			}
		}
	}

}

MyColor & RayManager::getColor(const Ray & ray, Scene & scene, int bounce) {
	if (bounce <= 0 || ray.intersectWith == nullptr)
	{
		return MyColor();
		//return scene.defColor;
	}

	else
	{
		std::vector<std::shared_ptr< Light>> effectiveLights = populateLights(ray, scene.lights, scene.bin);
		MyColor color = calcColor(ray, effectiveLights, scene.getAtt());
		//effectiveLights.clear();

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
	traceRay(reflectRay, scene.bin);
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
		Vec3 halfAngleToLight = ((ray.direction * -1.0f) + pointToLight);
		halfAngleToLight = halfAngleToLight.normalize();

		Material material = ray.intersectWith->mat;

		float attenuationValue = light->getAttValue(ray.getHitMin(), attenuation);
		pointToLight = pointToLight.normalize();
		result +=
			attenuationValue * (MyColor)(*(light->color)) *
			((material.diffuse * (pointToLight * normal)) +
				(material.specular * std::powf(halfAngleToLight * normal, material.shininess)));
	}
	return result;
}

std::vector<std::shared_ptr<Light>> RayManager::populateLights(const Ray & ray, std::vector<std::shared_ptr<Light>> allLights, std::shared_ptr<Container> bvh)
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

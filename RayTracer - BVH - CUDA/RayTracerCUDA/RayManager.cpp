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

				if (currBin->geo->isIntersecting(ray))
					ray.intersectWith = currBin->geo;
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
		MyColor color =
			ray.intersectWith->mat.refractValue *((MyColor(1, 1, 1) - ray.intersectWith->mat.reflectValue)* calcColor(ray, effectiveLights, scene.getAtt()) +
				ray.intersectWith->mat.reflectValue * getRefl(ray, scene, bounce - 1)) +
			(1 - ray.intersectWith->mat.refractValue) * getRefr(ray, scene, bounce - 1);
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
	return getColor(reflectRay, scene, bounce);
}

MyColor RayManager::getRefr(const Ray & ray, Scene & scene, int bounce) {
	if (ray.intersectWith->mat.refractValue != 1)
	{
		float cosI = ray.intersectWith->getNormal(ray.getHitReal()) * ray.direction;
		Vec3 normal;
		float n1, n2, n;
		normal = ray.intersectWith->getNormal(ray.getHitReal());
		if (cosI > 0)
		{
			n1 = ray.intersectWith->mat.refractIndex;
			n2 = 1;
			normal *= -1.0f;
		}
		else
		{
			n1 = 1;
			n2 = ray.intersectWith->mat.refractIndex;
			cosI = -cosI;
		}
		n = n1 / n2;
		float sinT2 = n * n * (1.0f - cosI * cosI);
		float cosT2 = (1.0f - sinT2);
		float cosT = (float)sqrtf(1.0f - sinT2);



		float rn = (n1 * cosI - n2 * cosT) / (n1 * cosI + n2 * cosT);
		float rt = (n2 * cosI - n1 * cosT) / (n2 * cosI + n2 * cosT);
		rn *= rn;
		rt *= rt;
		float refl = (rn + rt) * .5f;
		float trans = 1.0f - refl;

		if (cosT2 < 0)
		{
			//if (bounce - 1 == 0)
			//    return scene.defColor;
			return ray.intersectWith->mat.reflectValue *getRefl(ray, scene, bounce - 1);
		}

		Vec3 newDir = ray.direction * n + normal * (n * cosI - cosT);
		Ray refractRay(ray.getHitPlus(), newDir);
		refractRay.type = Ray::REFRACTION;
		traceRay(refractRay, scene.bin);
		return (getColor(refractRay, scene, bounce));
	}
	else return MyColor();
}

MyColor RayManager::calcColor(const Ray & ray, std::vector<std::shared_ptr<Light>> effectiveLights, Attenuation & attenuation)
{
	MyColor result = ray.intersectWith->mat.ambient + ray.intersectWith->mat.emmission;
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
			attenuationValue * (*(light->color)) *
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

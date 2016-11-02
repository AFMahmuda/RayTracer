#include "DirectionalLight.h"



DirectionalLight::DirectionalLight()
{
}


DirectionalLight::~DirectionalLight()
{
}

bool DirectionalLight::isEffective(Point3 & point, Container & bvh)
{
	Ray shadowRay = Ray(point, (*dir * -1.f));
	if (bvh.isIntersecting(shadowRay))
	{
		if (bvh.geo != nullptr)
		{
			shadowRay.transInv(bvh.geo->getTrans());

			if (bvh.geo->isIntersecting(shadowRay))
				return false;
		}
		else
		{

			if (!DirectionalLight::isEffective(point, *bvh.lChild))
				return false;
			if (!DirectionalLight::isEffective(point, *bvh.rChild))
				return false;

		}
	}

	return true;
	return false;
}

Vec3 DirectionalLight::getPointToLight(const Point3 & point)
{
	return Vec3(*dir * -1);
}

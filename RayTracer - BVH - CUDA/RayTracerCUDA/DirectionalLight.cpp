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
	if (bvh.IsIntersecting(shadowRay))
	{
		if (bvh.geo != nullptr)
		{
			if (bvh.geo->isIntersecting(shadowRay))
				return false;
		}
		else
		{

			if (!DirectionalLight::isEffective(point, *bvh.LChild))
				return false;
			if (!DirectionalLight::isEffective(point, *bvh.RChild))
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

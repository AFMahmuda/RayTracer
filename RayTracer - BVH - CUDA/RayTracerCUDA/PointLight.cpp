#include "PointLight.h"


PointLight::PointLight()
{
}


PointLight::~PointLight()
{
}

bool PointLight::isEffective(Point3 & point, Container & bvh)
{
	Vec3 pointToLight = getPointToLight(point);

	Ray shadowRay(point, pointToLight.Normalize());


	if (bvh.IsIntersecting(shadowRay))
	{
		if (bvh.geo != nullptr)
		{
			if (bvh.geo->isIntersecting(shadowRay))
				if (shadowRay.intersectDist < pointToLight.Magnitude())
					return false;
		}
		else
		{
			if (!PointLight::isEffective(point, *bvh.LChild))
				return false;
			if (!PointLight::isEffective(point, *bvh.RChild))
				return false;
		}
	}

	return true;
}

float PointLight::getAttValue(Point3 & point, Attenuation & att)
{
	float d = getPointToLight(Point3(point)).Magnitude();
	return 1.f / (att.cons + (att.line * d) + (att.line * d * d));
}

Vec3 PointLight::getPointToLight(const Point3 & point)
{
	return Vec3(point, Point3(*pos));
}

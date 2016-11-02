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
	Vec3& dir = pointToLight.Normalize();
	Ray shadowRay(point, dir);

	if (bvh.isIntersecting(shadowRay))
	{
		if (bvh.geo != nullptr)
		{
			//transform ray according to each shapes transformation
			shadowRay.transInv(bvh.geo->getTrans());
			if (bvh.geo->isIntersecting(shadowRay))
				if (shadowRay.intersectDist < pointToLight.Magnitude())
					return false;
		}
		else
		{
			if (!PointLight::isEffective(point, *bvh.lChild))
				return false;
			if (!PointLight::isEffective(point, *bvh.rChild))
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

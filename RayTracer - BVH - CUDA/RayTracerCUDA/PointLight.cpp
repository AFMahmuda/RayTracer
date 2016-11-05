#include "PointLight.h"


PointLight::PointLight()
{
}


PointLight::~PointLight()
{
}

bool PointLight::isEffective(vec3 & point, Container & bvh)
{
	vec3 pointToLight = getPointToLight(point);
	vec3 dir = pointToLight;
	dir = dir.Normalize();
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

float PointLight::getAttValue(vec3 & point, Attenuation & att)
{
	float d = getPointToLight(vec3(point)).Magnitude();
	return 1.f / (att.cons + (att.line * d) + (att.line * d * d));
}

vec3 PointLight::getPointToLight(const vec3 & point)
{
	return vec3(point, (*pos));
}

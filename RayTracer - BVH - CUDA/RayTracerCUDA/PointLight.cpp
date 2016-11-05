#include "PointLight.h"


PointLight::PointLight()
{
}


PointLight::~PointLight()
{
}

bool PointLight::isEffective(Data3D & point, Container & bvh)
{
	Data3D pointToLight = getPointToLight(point);
	Data3D dir = pointToLight;
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

float PointLight::getAttValue(Data3D & point, Attenuation & att)
{
	float d = getPointToLight(Data3D(point)).Magnitude();
	return 1.f / (att.cons + (att.line * d) + (att.line * d * d));
}

Data3D PointLight::getPointToLight(const Data3D & point)
{
	return Data3D(point, (*pos));
}

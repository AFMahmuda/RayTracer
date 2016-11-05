#include "DirectionalLight.h"



DirectionalLight::DirectionalLight()
{
}


DirectionalLight::~DirectionalLight()
{
}

bool DirectionalLight::isEffective(Data3D & point, Container & bvh)
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

Data3D DirectionalLight::getPointToLight(const Data3D & point)
{
	return Data3D(*dir * -1);
}

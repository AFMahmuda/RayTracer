#include "DirectionalLight.h"



DirectionalLight::DirectionalLight()
{
}


DirectionalLight::~DirectionalLight()
{
}

bool DirectionalLight::isEffective(Vec3 & point, std::shared_ptr< Container> bvh)
{
	Ray shadowRay = Ray(point, (*dir * -1.f));

	std::vector<std::shared_ptr< Container>> bins;

	bins.push_back(bvh);
	while (bins.size() > 0)
	{
		std::shared_ptr< Container> currBin = bins.back();
		bins.pop_back();
		if (currBin->isIntersecting(shadowRay))
		{
			if (currBin->geo != nullptr)
			{
				
				if (currBin->geo->isIntersecting(shadowRay))
					return false;
				
			}
			else
			{
				bins.push_back(currBin->rChild);
				bins.push_back(currBin->lChild);
			}
		}
	}

	return true;
}

Vec3 DirectionalLight::getPointToLight(const Vec3 & point)
{
	return Vec3(*dir * -1);
}

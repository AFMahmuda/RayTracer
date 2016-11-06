#include "DirectionalLight.h"



DirectionalLight::DirectionalLight()
{
}


DirectionalLight::~DirectionalLight()
{
}

bool DirectionalLight::isEffective(vec3 & point, std::shared_ptr< Container> bvh)
{
	Ray shadowRay = Ray(point, (*dir * -1.f));

	std::vector<std::shared_ptr< Container>> bins;

	bins.push_back(bvh);
	while (bins.size() > 0)
	{
		std::shared_ptr< Container> currBin = bins.back();
		if (currBin->isIntersecting(shadowRay))
		{
			if (currBin->geo != nullptr)
			{
				shadowRay.transInv(currBin->geo->getTrans());

				if (currBin->geo->isIntersecting(shadowRay))
					return false;
				//no need to transform ray back as ray no longer needed
			}
			else
			{

				//if (!DirectionalLight::isEffective(point, *bvh.lChild))
				//	return false;
				//if (!DirectionalLight::isEffective(point, *bvh.rChild))
				//	return false;
				bins.push_back(currBin->rChild);
				bins.push_back(currBin->lChild);
				std::vector<std::shared_ptr <Container>>::iterator lPos = std::find(bins.begin(), bins.end(), currBin->lChild);
				std::vector<std::shared_ptr <Container>>::iterator currPos = std::find(bins.begin(), bins.end(), currBin);
				std::swap(*lPos, *currPos);
			}
		}
		bins.pop_back();

	}

	return true;
}

vec3 DirectionalLight::getPointToLight(const vec3 & point)
{
	return vec3(*dir * -1);
}

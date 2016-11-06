#include "PointLight.h"


PointLight::PointLight()
{
}


PointLight::~PointLight()
{
}

bool PointLight::isEffective(vec3 & point, std::shared_ptr< Container> bvh)
{
	vec3 pointToLight = getPointToLight(point);
	vec3 dir = pointToLight;
	dir = dir.Normalize();
	Ray shadowRay(point, dir);
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
					if (shadowRay.intersectDist < pointToLight.Magnitude())
						return false;
			}
			else
			{
				//if (!PointLight::isEffective(point, *bvh.lChild))
				//	return false;
				//if (!PointLight::isEffective(point, *bvh.rChild))
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

float PointLight::getAttValue(vec3 & point, Attenuation & att)
{
	float d = getPointToLight(vec3(point)).Magnitude();
	return 1.f / (att.cons + (att.line * d) + (att.line * d * d));
}

vec3 PointLight::getPointToLight(const vec3 & point)
{
	return vec3(point, (*pos));
}

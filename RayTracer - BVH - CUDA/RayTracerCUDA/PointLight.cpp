#include "PointLight.h"


PointLight::PointLight()
{
}


PointLight::~PointLight()
{
}

bool PointLight::isEffective(Vec3 & point, std::shared_ptr< Container> bvh)
{
	Vec3 pointToLight = getPointToLight(point);
	Vec3 dir = pointToLight.normalize();
	Ray shadowRay(point, dir);
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
				Vec3 tempStart = shadowRay.start;
				Vec3 tempDir = shadowRay.direction;
				shadowRay.transInv(currBin->geo->getTrans());

				if (currBin->geo->isIntersecting(shadowRay))
					if (shadowRay.intersectDist < pointToLight.magnitude())
						return false;
				shadowRay.start = tempStart;
				shadowRay.direction = tempDir;
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

float PointLight::getAttValue(Vec3 & point, Attenuation & att)
{
	float d = getPointToLight(Vec3(point)).magnitude();
	return 1.f / (att.cons + (att.line * d) + (att.line * d * d));
}

Vec3 PointLight::getPointToLight(const Vec3 & point)
{
	return Vec3(point, (*pos));
}

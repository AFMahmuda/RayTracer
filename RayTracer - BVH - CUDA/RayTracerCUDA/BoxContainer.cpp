#include "BoxContainer.h"


BoxContainer::BoxContainer()
{
}


BoxContainer::~BoxContainer()
{
}

bool BoxContainer::IsIntersecting(Ray& ray)
{
	float tmin = -INFINITY, tmax = INFINITY;

	for (int i = 0; i < 3; i++)
	{		
		if (ray.direction[i] != 0.f)
		{
			float invTemp = 1.f / ray.direction[i];
			float tx1 = (min[i] - ray.start[i]) * invTemp;
			float tx2 = (max[i] - ray.start[i]) * invTemp;

			tmin = std::max(tmin, std::min(tx1, tx2));
			tmax = std::min(tmax, std::max(tx1, tx2));
		}
	}
	return tmax >= tmin;
}

void BoxContainer::showInfo()
{
	std::cout << "min: " << min[0] << "\t" << min[1] << "\t" << min[2] << std::endl;
	std::cout << "max: " << max[0] << "\t" << max[1] << "\t" << max[2] << std::endl;
}

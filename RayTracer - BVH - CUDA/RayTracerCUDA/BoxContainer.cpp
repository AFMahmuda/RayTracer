#include "BoxContainer.h"



BoxContainer::BoxContainer()
{
}


BoxContainer::~BoxContainer()
{
}

bool BoxContainer::IsIntersecting(Ray ray)
{
	float tmin = INFINITY, tmax = -INFINITY;

	//for (int i = 0; i < 3; i++)
	//{
	//	if (ray.Direction[i] != 0f)
	//	{
	//		float invTemp = 1f / ray.Direction[i];
	//		float tx1 = (min[i] - ray.Start[i]) * invTemp;
	//		float tx2 = (max[i] - ray.Start[i]) * invTemp;

	//		tmin = Math.Max(tmin, Math.Min(tx1, tx2));
	//		tmax = Math.Min(tmax, Math.Max(tx1, tx2));
	//	}
	//}

	return tmax >= tmin;
}

void BoxContainer::showInfo()
{

	std::cout << "min: " << min[0] << "\t" << min[1] << "\t" << min[2] << std::endl;
	std::cout << "max: " << max[0] << "\t" << max[1] << "\t" << max[2] << std::endl;


}

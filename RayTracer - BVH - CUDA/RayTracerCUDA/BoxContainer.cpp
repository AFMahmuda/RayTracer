#include <iostream>
#include "BoxContainer.h"
BoxContainer::BoxContainer(std::shared_ptr<Triangle> item)
{
	isLeaf = true;
	type = BOX;
	geo = item;

	
	{
		Triangle* tri = static_cast<Triangle*>(item.get());

		
		setMinMax(new Vec3[3]{ tri->a,tri-> b,tri->c }, 3);
	}
}

void BoxContainer::setMinMax(Vec3 * points, int n)
{
	min = Vec3(INFINITY, INFINITY, INFINITY, 1);
	max = Vec3(-INFINITY, -INFINITY, -INFINITY, 1);
	for (int i = 0; i < n; i++)
		for (int j = 0; j < 3; j++)
		{
			min[j] = std::min(min[j], points[i][j]);
			max[j] = std::max(max[j], points[i][j]);
		}
}

BoxContainer::BoxContainer(std::shared_ptr<BoxContainer>& a, std::shared_ptr<BoxContainer>& b)
{
	isLeaf = false;
	type = TYPE::BOX;
	lChild = (a);
	rChild = (b);


	for (int i = 0; i < 3; i++)
	{
		min[i] = std::min(a->min[i], b->min[i]);
		max[i] = std::max(a->max[i], b->max[i]);
	}
	Vec3 size = max - min;

	area = 2.f * (size[0] * size[1] + size[0] * size[2] + size[1] * size[2]);

}

BoxContainer::BoxContainer()
{
}


BoxContainer::~BoxContainer()
{
}

bool BoxContainer::isIntersecting(Ray& ray)
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

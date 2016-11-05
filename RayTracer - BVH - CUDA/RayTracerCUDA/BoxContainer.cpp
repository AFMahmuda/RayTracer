#include "BoxContainer.h"


BoxContainer::BoxContainer(std::shared_ptr<Geometry> item)
{
	isLeaf = true;
	type = BOX;
	geo = item;

	if (item->type == Geometry::SPHERE)
	{
		Sphere* sphere = (Sphere*)item.get();

		min = (sphere->c + vec3(0, 0, 0, 1));
		max = (sphere->c + vec3(0, 0, 0, 1));
		for (int i = 0; i < 3; i++)
		{
			min[i] -= sphere->r;
			max[i] += sphere->r;
		}

		vec3 p[8];

		p[0] = (vec3(min[0], min[1], min[2], 1));
		p[1] = (vec3(min[0], min[1], max[2], 1));
		p[2] = (vec3(min[0], max[1], min[2], 1));
		p[3] = (vec3(min[0], max[1], max[2], 1));
		p[4] = (vec3(max[0], min[1], min[2], 1));
		p[5] = (vec3(max[0], min[1], max[2], 1));
		p[6] = (vec3(max[0], max[1], min[2], 1));
		p[7] = (vec3(max[0], max[1], max[2], 1));


		for (int i = 0; i < 8; i++)
			p[i] = Matrix::Mul44x41(item->getTrans().matrix, p[i]);

		setMinMax(p, 8);

	}

	else //if (item.GetType() == typeof(Triangle))
	{
		Triangle* tri = static_cast<Triangle*>(item.get());

		vec3 a = Matrix::Mul44x41(item->getTrans().matrix, tri->a);
		vec3 b = Matrix::Mul44x41(item->getTrans().matrix, tri->b);
		vec3 c = Matrix::Mul44x41(item->getTrans().matrix, tri->c);

		setMinMax(new vec3[3]{ a, b, c }, 3);
	}
}

void BoxContainer::setMinMax(vec3 * points, int n)
{
	min = vec3(INFINITY, INFINITY, INFINITY,1);
	max = vec3(-INFINITY, -INFINITY, -INFINITY,1);
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
	vec3 size = max - min;

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

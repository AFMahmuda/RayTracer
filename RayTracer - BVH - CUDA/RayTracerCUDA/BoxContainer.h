#pragma once
#include "Container.h"
#include"Point3.h"
#include"Sphere.h"
#include"Triangle.h"

#include<algorithm>
class BoxContainer :
	public Container
{
public:
	Point3 min = Point3(INFINITY, INFINITY, INFINITY);
	Point3 max = Point3(-INFINITY, -INFINITY, -INFINITY);
	BoxContainer(Geometry* item)
	{
		isLeaf = true;
		type = BOX;
		geo = item;

		if (item->type == Geometry::SPHERE)
		{
			Sphere* sphere = (Sphere*)item;

			min = (sphere->c + Point3());
			max = (sphere->c + Point3());
			for (int i = 0; i < 3; i++)
			{
				min[i] -= sphere->r;
				max[i] += sphere->r;
			}

			Point3 p[8];

			p[0] = (Point3(min.x, min.y, min.z));
			p[1] = (Point3(min.x, min.y, max.z));
			p[2] = (Point3(min.x, max.y, min.z));
			p[3] = (Point3(min.x, max.y, max.z));
			p[4] = (Point3(max.x, min.y, min.z));
			p[5] = (Point3(max.x, min.y, max.z));
			p[6] = (Point3(max.x, max.y, min.z));
			p[7] = (Point3(max.x, max.y, max.z));


			for (int i = 0; i < 8; i++)
				p[i] = Matrix::Mul44x41(item->getTrans().matrix, p[i]);

			SetMinMax(p, 8);

		}

		else //if (item.GetType() == typeof(Triangle))
		{
			Triangle* tri = (Triangle*)item;

			Point3 a = Matrix::Mul44x41(item->getTrans().matrix, tri->a);
			Point3 b = Matrix::Mul44x41(item->getTrans().matrix, tri->b);
			Point3 c = Matrix::Mul44x41(item->getTrans().matrix, tri->c);

			SetMinMax(new Point3[3]{ a, b, c }, 3);
		}
		std::cout << "min: " << min[0] << "\t" << min[1] << "\t" << min[2] << std::endl;
		std::cout << "max: " << max[0] << "\t" << max[1] << "\t" << max[2] << std::endl;
	}

	void SetMinMax(Point3* points, int n)
	{
		min = Point3(INFINITY, INFINITY, INFINITY);
		max = Point3(-INFINITY, -INFINITY, -INFINITY);
		for (int i = 0; i < n; i++)
			for (int j = 0; j < 3; j++)
			{
				min[j] = std::min(min[j], points[i][j]);
				max[j] = std::max(max[j], points[i][j]);
			}
	}

	BoxContainer(BoxContainer& a, BoxContainer& b)
	{
		isLeaf = false;
		type = TYPE::BOX;
		LChild = &a;
		RChild = &b;


		for (int i = 0; i < 3; i++)
		{
			min[i] = std::min(a.min[i], b.min[i]);
			max[i] = std::max(a.max[i], b.max[i]);
		}
		Point3 size = max - min;

		area = 2.f * (size.x * size.y + size.x * size.z + size.y * size.z);

	}

	BoxContainer();
	~BoxContainer();



	// Inherited via Container
	virtual bool IsIntersecting(Ray ray) override;
	virtual void showInfo() override;
};


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
	IData3D min = Point3(INFINITY, INFINITY, INFINITY);
	IData3D max = Point3(-INFINITY, -INFINITY, -INFINITY);
	BoxContainer(std::shared_ptr<Geometry> item)
	{
		isLeaf = true;
		type = BOX;
		geo = item;

		if (item->type == Geometry::SPHERE)
		{
			Sphere* sphere = (Sphere*)item.get();

			min = (sphere->c + Point3());
			max = (sphere->c + Point3());
			for (int i = 0; i < 3; i++)
			{
				min[i] -= sphere->r;
				max[i] += sphere->r;
			}

			IData3D p[8];

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
			Triangle* tri = static_cast<Triangle*>(item.get());

			IData3D a = Matrix::Mul44x41(item->getTrans().matrix, tri->a);
			IData3D b = Matrix::Mul44x41(item->getTrans().matrix, tri->b);
			IData3D c = Matrix::Mul44x41(item->getTrans().matrix, tri->c);

			SetMinMax(new IData3D[3]{ a, b, c }, 3);
		}
	}

	void SetMinMax(IData3D* points, int n)
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
		LChild = std::make_shared<BoxContainer>(a);
		RChild = std::make_shared<BoxContainer>(b);


		for (int i = 0; i < 3; i++)
		{
			min[i] = std::min(a.min[i], b.min[i]);
			max[i] = std::max(a.max[i], b.max[i]);
		}
		IData3D size = max - min;

		area = 2.f * (size.x * size.y + size.x * size.z + size.y * size.z);

	}

	BoxContainer();
	~BoxContainer();

	// Inherited via Container
	virtual bool IsIntersecting(Ray& ray) override;
	virtual void showInfo() override;
};


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
	Point3 min = Point3(FLT_MAX, FLT_MAX, FLT_MAX);
	Point3 max = Point3(FLT_MIN, FLT_MIN, FLT_MIN);
	BoxContainer(Geometry* item)
	{
		type = BOX;
		geo = item;

		if (item->type == Geometry::SPHERE)
		{
			Sphere* sphere = (Sphere*)item;

//			min = Utils.DeepClone(sphere.c);
			min = sphere->c + Point3();
			min.x -= sphere->r;
			min.y -= sphere->r;
			min.z -= sphere->r;

			max = sphere->c + Point3();
			max.x += sphere->r;
			max.y += sphere->r;
			max.z += sphere->r;

			for (int i = 0; i < 3; i++)
			{
				min[i] -= sphere->r;
				max[i] += sphere->r;
			}

			Point3 p[8];

			p[0] = ( Point3(min.x, min.y, min.z));
			p[1] = ( Point3(min.x, min.y, max.z));
			p[2] = ( Point3(min.x, max.y, min.z));
			p[3] = ( Point3(min.x, max.y, max.z));
			p[4] = ( Point3(max.x, min.y, min.z));
			p[5] = ( Point3(max.x, min.y, max.z));
			p[6] = ( Point3(max.x, max.y, min.z));
			p[7] = ( Point3(max.x, max.y, max.z));


			for (int i = 0; i < sizeof(p)/sizeof(Point3); i++)
				p[i] = Matrix::Mul44x41(item->getTrans().matrix, p[i]);


			SetMinMax(p,8);

		}

		else //if (item.GetType() == typeof(Triangle))
		{
			Triangle* tri = (Triangle*)item;

			Point3 a = Matrix::Mul44x41(item->getTrans().matrix, tri->a);
			Point3 b = Matrix::Mul44x41(item->getTrans().matrix, tri->b);
			Point3 c = Matrix::Mul44x41(item->getTrans().matrix, tri->c);

			SetMinMax(new Point3[3]{ a, b, c },3);
		}
	}

	void SetMinMax(Point3* points,int n)
	{
		 min = Point3(FLT_MAX, FLT_MAX, FLT_MAX);
		max = Point3(FLT_MIN, FLT_MIN, FLT_MIN);
		for (int i = 0; i < n; i++)
			for (int j = 0; j < 3; j++)
			{
				min[j] = std::min(min[j], points[i][j]);
				max[j] = std::max(max[j], points[i][j]);
			}
	}

	bool IsIntersecting(Ray* ray) { 
		float tmin = FLT_MIN, tmax = FLT_MAX;

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
	BoxContainer();
	~BoxContainer();
};


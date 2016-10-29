#pragma once
#include"Container.h"
#include"Point3.h"
#include"Sphere.h"
#include"Triangle.h"

#include<algorithm>
#ifndef M_PI
#define M_PI 3.14159265358979323846
#endif


class SphereContainer :
	public Container
{
public:
	Point3 c;
	float r;

	SphereContainer(Geometry* item) {
		type = SPHERE;
		geo = item;
		if (item->type == Geometry::SPHERE)
		{

			c = ((Sphere*)item)->c;
			r = ((Sphere*)item)->r;

		}
		else //if (item.GetType() == typeof(Triangle))
		{
			Triangle& tri = *(Triangle*)item;
			Vec3 ab =Vec3(tri.a, tri.b);
			Vec3 bc = Vec3(tri.b, tri.c);
			Vec3 ac = Vec3(tri.a, tri.c);

			float d = 2 * ((ab * ab) * (ac * ac) - (ab * ac) * (ab * ac));
			Point3 reference = tri.a;
			float s = ((ab * ab) * (ac * ac) - (ac * ac) * (ab * ac)) / d;
			float t = ((ac * ac) * (ab * ab) - (ab * ab) * (ab * ac)) / d;
			if (s <= 0)
			{
				c = (tri.a + tri.c) * .5f;
			}
			else if (t <= 0)
			{
				c = (tri.a + tri.b) * .5f;
			}
			else if (s + t > 1)
			{
				c = (tri.b + tri.c) * .5f;
				reference = tri.b;
			}
			else c = tri.a + (tri.b - tri.a) * s + (tri.c - tri.a) * t;
			r = sqrtf(Vec3(reference, tri.c) * Vec3(reference, tri.c));
		}

		c = Matrix::Mul44x41(item->getTrans().matrix, c);
		r = Matrix::Mul44x41(item->getTrans().matrix, Vec3(r, 0, 0)).Magnitude();
	}
	SphereContainer() {}
	SphereContainer(SphereContainer& a, SphereContainer& b)
	{
		type = SPHERE;
		childs = new SphereContainer[2];
		childs[0] = a;
		childs[1] = b;

		Vec3 aToB = Vec3(a.c, b.c);
		float aToBLength = aToB.Magnitude();

		if (aToB.Magnitude() == 0)
		{
			r = std::max(a.r, b.r);
			c = a.c;
		}

		else if (aToBLength + a.r + b.r < a.r * 2 ||
			aToB.Magnitude() + a.r + b.r < b.r * 2)
		{
			r = std::max(a.r, b.r);
			c = (a.r > b.r) ? a.c : b.c;
		}

		else
		{
			r = (a.r + b.r + aToB.Magnitude()) * .5f;
			c = a.c + (aToB.Normalize() * (r - a.r));
		}

		area = 4.f * (float)M_PI * (float)std::powf(r, 2);
	}
	bool IsIntersecting(Ray* ray)
	{

		//Vec3 rayToSphere = Vec3(ray->start, this.c);

		//float a = ray.Direction * ray.Direction;
		//float b = -2 * (rayToSphere * ray.Direction);
		//float c = (rayToSphere * rayToSphere) - (r * r);
		//float dd = (b * b) - (4 * a * c);

		//return (dd > 0);
		return false;
	}
	~SphereContainer();
};


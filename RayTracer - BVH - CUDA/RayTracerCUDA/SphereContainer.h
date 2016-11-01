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
	Data3D c;
	float r;

	SphereContainer(std::shared_ptr< Geometry> item) {
		isLeaf = true;
		type = SPHERE;
		geo = item;
		if (item->type == Geometry::SPHERE)
		{

			c = ((Sphere*)item.get())->c + Point3();
			r = ((Sphere*)item.get())->r + 0.f;

		}
		else //if (item.GetType() == typeof(Triangle))
		{
			Triangle& tri = *(Triangle*)item.get();
			Vec3 ab = Vec3(tri.a, tri.b);
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
		r = Vec3(Matrix::Mul44x41(item->getTrans().matrix, Vec3(r, 0, 0))).Magnitude();

	}
	SphereContainer() {}
	SphereContainer(SphereContainer& a, SphereContainer& b)
	{
		isLeaf = false;

		type = SPHERE;

		LChild = std::make_shared<SphereContainer>(a);
		RChild = std::make_shared<SphereContainer>(b);

		Vec3 aToB = Vec3(a.c, b.c);
		float aToBLength = aToB.Magnitude();

		if (aToB.Magnitude() == 0)
		{
			r = std::max(a.r, b.r);
			c = a.c + Point3();
		}

		else if (aToBLength + a.r + b.r < a.r * 2 ||
			aToB.Magnitude() + a.r + b.r < b.r * 2)
		{
			r = std::max(a.r, b.r);
			c = (a.r > b.r) ? a.c + Point3() : b.c + Point3();
		}

		else
		{
			r = (a.r + b.r + aToB.Magnitude()) * .5f;

			aToB = aToB.Normalize();
			aToB = aToB * (r - a.r);
			c = (a.c) + Point3(aToB.x, aToB.y, aToB.z);
		}

		area = 4.f * (float)M_PI * (float)std::powf(r, 2);
	}

	~SphereContainer();



	// Inherited via Container
	virtual bool IsIntersecting(Ray& ray) override;
	virtual void showInfo() override;
};


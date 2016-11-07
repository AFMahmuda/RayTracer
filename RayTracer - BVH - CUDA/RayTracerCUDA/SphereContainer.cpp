#include "SphereContainer.h"
#include<algorithm>
#include <iostream>
#ifndef M_PI
#define M_PI 3.14159265358979323846
#endif

SphereContainer::SphereContainer(std::shared_ptr<Triangle> item) {
	isLeaf = true;
	type = SPHERE;
	geo = item;

	{
		Triangle& tri = *item;
		Vec3 ab(tri.a, tri.b);
		Vec3 bc(tri.b, tri.c);
		Vec3 ac(tri.a, tri.c);

		float d = 2 * ((ab * ab) * (ac * ac) - (ab * ac) * (ab * ac));
		Vec3 reference = tri.a;
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
}

SphereContainer::SphereContainer(std::shared_ptr<SphereContainer> a, std::shared_ptr<SphereContainer> b)
{
	isLeaf = false;
	type = SPHERE;
	lChild = a;
	rChild = b;

	Vec3 aToB(a->c, b->c);
	float aToBLength = aToB.magnitude();

	if (aToB.magnitude() == 0)
	{
		r = std::max(a->r, b->r);
		c = a->c + Vec3(0, 0, 0, 1);
	}

	else if (aToBLength + a->r + b->r < a->r * 2.f ||
		aToB.magnitude() + a->r + b->r < b->r * 2.f)
	{
		r = std::max(a->r, b->r);
		c = (a->r > b->r) ? a->c + Vec3(0, 0, 0, 1) : b->c + Vec3(0, 0, 0, 1);
	}

	else
	{
		r = (a->r + b->r + aToB.magnitude()) * .5f;

		aToB = aToB.normalize();
		aToB = aToB * (r - a->r);
		c = (a->c) + Vec3(aToB[0], aToB[1], aToB[2], 1);
	}

	area = 4.f * (float)M_PI * (float)std::powf(r, 2);
}

SphereContainer::~SphereContainer()
{
}

bool SphereContainer::isIntersecting(Ray& ray)
{
	Vec3 rayToSphere(ray.start, this->c);

	float a = ray.direction * ray.direction;
	float b = -2 * (rayToSphere * ray.direction);
	float c = (rayToSphere * rayToSphere) - (r * r);
	float dd = (b * b) - (4 * a * c);

	return (dd > 0);
}

void SphereContainer::showInfo()
{
	std::cout << "c : " << c[0] << "\t" << c[1] << "\t" << c[2] << "\tr: " << r << std::endl;
}

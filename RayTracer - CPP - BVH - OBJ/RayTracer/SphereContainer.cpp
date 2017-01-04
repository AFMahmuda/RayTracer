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

	Triangle& tri = *item;
	Vec3 vecs[3] = { tri.a,tri.b,tri.c };
	c = vecs[0];
	r = 0.0001f;

	Vec3 pos, move;
	float len;

	for (int j = 0; j < 3; j++) {
		pos = vecs[j];
		move = pos - c;
		len = move.magnitude();
		if (len > r) {
			r = (r + len) / 2.0f;
			c = c + ((len - r) / len * move);
		}
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

	if (aToB.magnitude() == 0)	{ //same center diff r
		r = std::max(a->r, b->r);
		c = a->c;
	}

	else if ( //one sphere inside the other sphere
		aToBLength + a->r + b->r < a->r * 2.f ||
		aToBLength + a->r + b->r < b->r * 2.f )
	{
		r = std::max(a->r, b->r);
		c = (a->r > b->r) ? a->c  : b->c ;
	}

	else {
		r = (a->r + b->r + aToBLength) * .5f;

		aToB = aToB.normalize();
		aToB = aToB * (r - a->r);
		c = (a->c) + aToB;
	}

	area = 4.f * (float)M_PI * (float)std::powf(r, 2.f);
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

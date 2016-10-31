#include "Sphere.h"





Sphere::~Sphere()
{
}

void Sphere::updatePos()
{

	pos = Point3();
	pos = Matrix::Mul44x41(trans.matrix, c);

	Point3 p = pos;
	p[0] = (p[0] / 100.f) + .5f;
	p[1] = (p[1] / 100.f) + .5f;
	p[2] = (p[2] / 100.f) + .5f;

	pos = p;
	getMortonPos();

}

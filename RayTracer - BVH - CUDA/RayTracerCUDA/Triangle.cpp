#include "Triangle.h"



Triangle::Triangle()
{
}




Triangle::~Triangle()
{
}

void Triangle::updatePos()
{
	Point3 temp = (a + b + c)* (.33f);
	pos = Matrix::Mul44x41(trans.matrix, temp);
	pos[1] = (pos[0] / 100.f + .5f);
	pos[1] = (pos[1] / 100.f + .5f);
	pos[1] = (pos[1] / 100.f + .5f);

	getMortonPos();
}

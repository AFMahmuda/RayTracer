#include "Point3.h"

Point3::Point3(float x, float y, float z)
{
	Point3::x = x;
	Point3::y = y;
	Point3::z = z;
	h = 1;
}

Point3 Point3::ZERO()
{
	return Point3(0, 0, 0);
}



Point3::~Point3()
{
}

//float Point3::getX()
//{
//	return x;
//}
//
//float Point3::getY()
//{
//	return y;
//}
//
//float Point3::getZ()
//{
//	return z;
//}


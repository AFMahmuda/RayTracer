#include "Vec3.h"
Vec3::Vec3(float * params) : Vec3(params[0], params[1], params[2]) {}
 Vec3::Vec3(const IData3D & point) : Vec3(point[0], point[1], point[2]) {}
 Vec3::Vec3(const IData3D & start, const IData3D & end) : Vec3(end[0] - start[0], end[1] - start[1], end[2] - start[2]) {}

float Vec3::Magnitude()
{
	return sqrtf(x * x + y * y + z * z);
}

Vec3& Vec3::Normalize()
{
	float divMag =1.f/ Magnitude();

	return Vec3(x * divMag, y * divMag, z * divMag);

}

Vec3 Vec3::Cross(const Vec3 & a, const Vec3 & b) {
	float X, Y, Z;
	X = (a.y * b.z) - (a.z * b.y);
	Y = ((a.x * b.z) - (a.z * b.x)) * -1;
	Z = (a.x * b.y) - (a.y * b.x);

	Vec3& res = Vec3(X, Y, Z);
	return res;
}

Vec3::~Vec3()
{
}

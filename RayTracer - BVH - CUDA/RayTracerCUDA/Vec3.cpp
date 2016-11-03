#include "Vec3.h"
Vec3::Vec3(float * params) : Vec3(params[0], params[1], params[2]) {}
 Vec3::Vec3(const Data3D & point) : Vec3(point[0], point[1], point[2]) {}
 Vec3::Vec3(const Data3D & start, const Data3D & end) : Vec3(end[0] - start[0], end[1] - start[1], end[2] - start[2]) {}

float Vec3::Magnitude()
{
	return sqrtf(v[0] * v[0] + v[1] * v[1] + v[2] * v[2]);
}

Vec3& Vec3::Normalize()
{
	float divMag =1.f/ Magnitude();

	return Vec3(v[0] * divMag, v[1] * divMag, v[2] * divMag);

}

Vec3 Vec3::Cross(const Vec3 & a, const Vec3 & b) {
	float X, Y, Z;
	X = (a.v[1] * b.v[2]) - (a.v[2] * b.v[1]);
	Y = ((a.v[0] * b.v[2]) - (a.v[2] * b.v[0])) * -1;
	Z = (a.v[0] * b.v[1]) - (a.v[1] * b.v[0]);

	Vec3& res = Vec3(X, Y, Z);
	return res;
}

Vec3::~Vec3()
{
}

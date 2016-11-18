#include "Vec3.h"
#include <iostream>



Vec3& Vec3::operator+=(const Vec3& rhs) {

	for (size_t i = 0; i < 3; i++)
	{
		v[i] += rhs[i];
	}
	return *this;
}

Vec3& Vec3::operator-=(const Vec3& rhs) {

	for (size_t i = 0; i < 3; i++)
	{
		v[i] -= rhs[i];
	}

	return *this;
}


Vec3& Vec3::operator*=(float rhs) {

	for (size_t i = 0; i < 3; i++)
	{
		v[i] *= rhs;
	}

	return *this;
}


float Vec3::magnitude()
{
	return sqrtf(v[0] * v[0] + v[1] * v[1] + v[2] * v[2]);
}

Vec3 Vec3::normalize()
{
	float divMag = 1.f / magnitude();

	return Vec3(v[0] * divMag, v[1] * divMag, v[2] * divMag, 0);

}

Vec3 Vec3::Cross(const Vec3 & a, const Vec3 & b)
{
	float newX = a[1] * b[2] - a[2] * b[1];
	float newY = a[2] * b[0] - a[0] * b[2];
	float newZ = a[0] * b[1] - a[1] * b[0];
	return Vec3(newX, newY, newZ, 0);
}

void Vec3::show() {
	std::cout << v[0] << " " << v[1] << " " << v[2];
}

Vec3& Vec3::operator=(const Vec3& other){
	memcpy(v, other.v, 4 * sizeof(float));
	return *this;
}
Vec3::Vec3(float * vals) :Vec3()
{
	memcpy(v, vals, 4 * sizeof(float));
}
Vec3::Vec3(const Vec3 & ori) : Vec3() 
{
	memcpy(v, ori.v, 4 * sizeof(float));
}

Vec3::Vec3(float x, float y, float z, float h) : Vec3()
{
	v[0] = x; v[1] = y; v[2] = z; v[3] = h;
}

Vec3::Vec3(float * vals, float h) : Vec3(vals[0], vals[1], vals[2], h)
{
}
Vec3::Vec3(Vec3 a, Vec3 b) : Vec3(b - a)
{
	v[3] = 0;
}

Vec3::Vec3() : v(new float[4]) {
	v[0] = 0; v[1] = 0; v[2] = 0; v[3] = 0;
}

Vec3::~Vec3()
{
	delete[]v;
}

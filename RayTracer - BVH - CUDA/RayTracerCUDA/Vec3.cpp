#include "Vec3.h"
#include <cuda_runtime.h>
#include "cublas_v2.h"
#include <iostream>
#include "MyCUDAHandler.h"

vec3& vec3::operator+=(const vec3& rhs) {

	for (size_t i = 0; i < 3; i++)
	{
		v[i] += rhs[i];
	}
	return *this;
}

vec3& vec3::operator-=(const vec3& rhs) {

	for (size_t i = 0; i < 3; i++)
	{
		v[i] -= rhs[i];
	}

	return *this;
}


vec3& vec3::operator*=(float rhs) {

	for (size_t i = 0; i < 3; i++)
	{
		v[i] *= rhs;
	}

	return *this;
}

vec3::vec3(float * vals) :vec3()
{
	memcpy(v, vals, 4 * sizeof(float));
}

float vec3::Magnitude()
{
	return sqrtf(v[0] * v[0] + v[1] * v[1] + v[2] * v[2]);
}

vec3 vec3::Normalize()
{
	float divMag = 1.f / Magnitude();

	return vec3(v[0] * divMag, v[1] * divMag, v[2] * divMag, 1);

}

vec3 vec3::Cross(const vec3 & a, const vec3 & b)
{
	float newX = a[1] * b[2] - a[2] * b[1];
	float newY = a[2] * b[0] - a[0] * b[2];
	float newZ = a[0] * b[1] - a[1] * b[0];
	return vec3(newX, newY, newZ, 0);
}

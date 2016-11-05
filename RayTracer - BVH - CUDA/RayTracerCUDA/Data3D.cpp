#include "Data3D.h"
#include <cuda_runtime.h>
#include "cublas_v2.h"
#include <iostream>
#include "MyCUDAHandler.h"

Data3D& Data3D::operator+=(const Data3D& rhs) {

	for (size_t i = 0; i < 3; i++)
	{
		v[i] += rhs[i];
	}
	return *this;
}

Data3D& Data3D::operator-=(const Data3D& rhs) {

	for (size_t i = 0; i < 3; i++)
	{
		v[i] -= rhs[i];
	}

	return *this;
}


Data3D& Data3D::operator*=(float rhs) {

	for (size_t i = 0; i < 3; i++)
	{
		v[i] *= rhs;
	}

	return *this;
}

Data3D::Data3D(float * vals) :Data3D()
{
	memcpy(v, vals, 4 * sizeof(float));
}

float Data3D::Magnitude()
{
	return sqrtf(v[0] * v[0] + v[1] * v[1] + v[2] * v[2]);
}

Data3D Data3D::Normalize()
{
	float divMag = 1.f / Magnitude();

	return Data3D(v[0] * divMag, v[1] * divMag, v[2] * divMag, 1);

}

Data3D Data3D::Cross(const Data3D & a, const Data3D & b)
{
	float newX = a[1] * b[2] - a[2] * b[1];
	float newY = a[2] * b[0] - a[0] * b[2];
	float newZ = a[0] * b[1] - a[1] * b[0];
	return Data3D(newX, newY, newZ, 0);
}

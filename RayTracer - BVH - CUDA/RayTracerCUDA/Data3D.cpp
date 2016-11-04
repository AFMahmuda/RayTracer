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

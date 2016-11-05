#include "Rotation.h"
//#define _USE_MATH_DEFINES
//#include <cmath>  
#ifndef M_PI
#define M_PI 3.14159265358979323846
#endif
Rotation::Rotation(float * xyz, float deg)
{
	matrix = Matrix(4, 4);
	float rad = deg * (float) M_PI / 180.0f;

	float x = xyz[0] * rad;
	float cx = cosf(x);
	float sx = sinf(x);
	float y = xyz[1] * rad;
	float cy = cosf(y);
	float sy = sinf(y);
	float z = xyz[2] * rad;
	float cz = cosf(z);
	float sz = sinf(z);

	matrix(0, 0) = cy * cz;
	matrix(0, 1) = (cz * sx * sy - cx * sz);
	matrix(0, 2) = (cx * cz * sy + sx * sz);

	matrix(1, 0) = (cy * sz);
	matrix(1, 1) = (cx * cz + sx * sy * sz);
	matrix(1, 2) = (-cz * sx + cx * sy * sz);

	matrix(2, 0) = (-sy);
	matrix(2, 1) = (cy * sx);
	matrix(2, 2) = (cx * cy);

	matrix(3, 3) = (1.f);
}


Rotation::~Rotation()
{
}

#include "Scaling.h"



Scaling::Scaling(float x, float y, float z)
{
	matrix = Matrix(4, 4);
	matrix(0, 0) = x;
	matrix(1, 1) = y;
	matrix(2, 2) = z;
	matrix(3, 3) = 1.f;

}

Scaling::~Scaling()
{
}

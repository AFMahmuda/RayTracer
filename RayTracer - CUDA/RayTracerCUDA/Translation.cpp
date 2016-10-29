#include "Translation.h"





Translation::Translation(float x, float y, float z)
{

	matrix = Matrix(4, 4);
	matrix = matrix.Identity();
	matrix(0, 3) = x;
	matrix(1, 3) = y;
	matrix(2, 3) = z;
	matrix(3, 3) = 1.f;
}

Translation::~Translation()
{
}

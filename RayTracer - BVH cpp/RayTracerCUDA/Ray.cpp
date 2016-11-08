#include "Ray.h"



void Ray::trans(Transform & transform) {
	start = (Matrix::Mul44x41(transform.matrix, start));
	direction = Vec3(Matrix::Mul44x41(transform.matrix, direction)).normalize();
}

void Ray::transInv(Transform & transform) {
	start = (Matrix::Mul44x41(Matrix(transform.matrix.Inverse()), start));
	direction = Vec3(Matrix::Mul44x41(Matrix(transform.matrix.Inverse()), direction)).normalize();
}

bool Ray::isCloser(float dist, Transform & trans)
{
	float newMag = (Matrix::Mul44x41(trans.matrix, direction * dist)).magnitude();
	return (newMag < intersectDist) ? true : false;
}

Ray::Ray()
{
}


Ray::~Ray()
{
}

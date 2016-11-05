#include "Ray.h"



void Ray::trans(Transform & transform) {
	start = (Matrix::Mul44x41(transform.matrix, start));
	direction = vec3(Matrix::Mul44x41(transform.matrix, direction)).Normalize();
}

void Ray::transInv(Transform & transform) {
	start = (Matrix::Mul44x41((Matrix)transform.matrix.Inverse(), start));
	direction = vec3(Matrix::Mul44x41((Matrix)transform.matrix.Inverse(), direction)).Normalize();
}

bool Ray::isCloser(float dist, Transform & trans)
{
	float newMag = (Matrix::Mul44x41(trans.matrix, direction * dist)).Magnitude();
	return (newMag < intersectDist) ? true : false;
}

Ray::Ray()
{
}


Ray::~Ray()
{
}

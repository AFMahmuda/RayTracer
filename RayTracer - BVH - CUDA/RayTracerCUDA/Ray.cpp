#include "Ray.h"



void Ray::trans(Transform & transform) {
	start = Point3(Matrix::Mul44x41(transform.matrix, start));
	direction = Vec3(Matrix::Mul44x41(transform.matrix, direction)).Normalize();
}

void Ray::transInv(Transform & transform) {
	start = Point3(Matrix::Mul44x41((Matrix)transform.matrix.Inverse(), start));
	direction = Vec3(Matrix::Mul44x41((Matrix)transform.matrix.Inverse(), direction)).Normalize();

}

bool Ray::isCloser(float dist, Transform & trans)
{
	float newMag = Vec3(Matrix::Mul44x41(trans.matrix, direction * dist)).Magnitude();
	return (newMag < intersectDist) ? true : false;
}

Ray::Ray()
{
}


Ray::~Ray()
{
}

#include "Ray.h"



Vec3 Ray::getHit() const { return start + direction * intersectDist; }

Vec3 Ray::getHitPlus() const { return start + direction * (intersectDist + epsilon); }

Vec3 Ray::getHitMin() const { return start + direction * (intersectDist - epsilon); }

bool Ray::isCloser(float dist)
{
	float newMag = (direction * dist).magnitude();
	return (newMag < (intersectDist - epsilon) && newMag > epsilon) ? true : false;
}

Ray::Ray()
{
	epsilon = 0.f;
}

Ray::Ray(Vec3 start, Vec3 dir) : start(start), direction(dir) {
	epsilon = 0.0001f;
}


Ray::~Ray()
{
}

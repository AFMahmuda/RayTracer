#include "Ray.h"



Vec3 Ray::getHit() const { return start + direction * intersectDist; }

//Vec3 Ray::getHitPlus() const { return start + direction * (intersectDist + epsilon); }
Vec3 Ray::getHitPlus() const { return start + direction * (intersectDist * 1.00001f); }

Vec3 Ray::getHitMin() const { return start + direction * (intersectDist - epsilon); }

bool Ray::isCloser(float dist)
{
	float newMag = (direction * dist).magnitude();
	return (newMag < (intersectDist - epsilon) && newMag > epsilon) ? true : false;
}

Ray::Ray()
{
	hitCount[0] = 0;
	hitCount[1] = 0;
}

Ray::Ray(Vec3 start, Vec3 dir) : start(start), direction(dir) {
	hitCount[0] = 0;
	hitCount[1] = 0;
}


Ray::~Ray()
{
}

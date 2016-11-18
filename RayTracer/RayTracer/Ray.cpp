#include "Ray.h"



bool Ray::isCloser(float dist)
{
	float newMag =  (direction * dist).magnitude();
	return (newMag < intersectDist) ? true : false;
}

Ray::Ray()
{
}


Ray::~Ray()
{
}

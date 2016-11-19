#pragma once

#include<memory>
#include<vector>

#include "Vec3.h"

class Triangle;//forward declaration
class Ray
{
public:
	enum  TYPE {
		RAY, REFLECTION, REFRACTION
	};

	TYPE type;
	Vec3 start;
	Vec3 direction;
	float intersectDist = FLT_MAX;
	float epsilon;
	std::shared_ptr< Triangle > intersectWith;
	Vec3 getHit() const;
	Vec3 getHitPlus() const;
	Vec3 getHitMin() const;

	bool isCloser(float dist);

	Ray();
	Ray(Vec3 start, Vec3 dir);
	~Ray();
};



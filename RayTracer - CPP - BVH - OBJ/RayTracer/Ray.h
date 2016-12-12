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
	float epsilon = 0.0001f;
	std::shared_ptr< Triangle > intersectWith;
	Vec3 getHit() const;
	Vec3 getHitPlus() const;
	Vec3 getHitMin() const;

	bool isCloser(float dist);
	int hitCount[2];
	Ray();
	Ray(Vec3 start, Vec3 dir);
	~Ray();
};



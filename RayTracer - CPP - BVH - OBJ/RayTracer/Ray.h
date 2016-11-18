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
	std::shared_ptr< Triangle > intersectWith;
	Vec3 getHitReal() const { return Vec3(start + Vec3(direction * intersectDist)); }
	Vec3 getHitPlus() const { return Vec3(start + Vec3(direction * (intersectDist * 1.0001f))); }
	Vec3 getHitMin() const { return Vec3(start + Vec3(direction * (intersectDist * 0.9999f))); }


	bool isCloser(float dist);

	Ray();
	Ray(Vec3 start, Vec3 dir) :start(start), direction(dir) {}
	~Ray();
};



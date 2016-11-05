#pragma once

#include<memory>
#include<vector>

//#include"Scene.h"
//#include"Container.h"
//#include"Light.h"
//#include"MyColor.h"
//#include"Matrix.h"

#include"Transform.h"

#include "Vec3.h"

class Geometry;//forward declaration
class Ray
{
public:
	enum  TYPE {
		RAY, REFLECTION, REFRACTION
	};

	TYPE type;
	vec3 start;
	vec3 direction;
	float intersectDist = FLT_MAX;
	std::shared_ptr< Geometry > intersectWith;
	vec3 getHitReal() const { return vec3(start + vec3(direction * intersectDist)); }
	vec3 getHitPlus() const { return vec3(start + vec3(direction * (intersectDist * 1.01f))); }
	vec3 getHitMin() const { return vec3(start + vec3(direction * (intersectDist * 0.99f))); }

	void trans(Transform& transform);
	void transInv(Transform& transform);
	bool isCloser(float dist, Transform& trans);

	Ray();
	Ray(vec3 start, vec3 dir) :start(start), direction(dir) {}
	~Ray();
};



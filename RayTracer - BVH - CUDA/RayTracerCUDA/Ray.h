#pragma once

#include<memory>
#include<vector>

//#include"Scene.h"
//#include"Container.h"
//#include"Light.h"
//#include"MyColor.h"
//#include"Matrix.h"

#include"Transform.h"

#include"Point3.h"
#include"Vec3.h"

class Geometry;//forward declaration
class Ray
{
public:
	enum  TYPE {
		RAY, REFLECTION, REFRACTION
	};

	TYPE type;
	Point3 start;
	Vec3 direction;
	float intersectDist = FLT_MAX;
	std::shared_ptr< Geometry > intersectWith;
	Point3 getHitReal() const { return Point3(start + Point3(direction * intersectDist)); }
	Point3 getHitPlus() const { return Point3(start + Point3(direction * (intersectDist * 1.01f))); }
	Point3 getHitMin() const { return Point3(start + Point3(direction * (intersectDist * 0.99f))); }

	void trans(Transform& transform);
	void transInv(Transform& transform);
	bool isCloser(float dist, Transform& trans);

	Ray();
	Ray(Point3 start, Vec3 dir) :start(start), direction(dir) {}
	~Ray();
};



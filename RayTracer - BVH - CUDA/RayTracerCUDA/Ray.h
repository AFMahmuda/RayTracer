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
	float intersectDist = INT_MAX;
	std::shared_ptr< Geometry > intersectWith;
	Point3 getHitReal() const { return Point3(start + Point3(direction * intersectDist)); }
	Point3 getHitPlus() const { return Point3(start + Point3(direction * (intersectDist * 1.01))); }
	Point3 getHitMin() const { return Point3(start + Point3(direction * (intersectDist * 0.99))); }

	void trans(Transform& transform) {
		start = Point3(Matrix::Mul44x41(transform.matrix, start));
		direction = Vec3(Matrix::Mul44x41(transform.matrix, direction)).Normalize();
	}
	void transInv(Transform& transform) {
		start = Point3(Matrix::Mul44x41((Matrix)transform.matrix.Inverse(), start));
		direction = Vec3(Matrix::Mul44x41((Matrix)transform.matrix.Inverse(), direction)).Normalize();

	}
	bool isCloser(float dist, Transform& trans)
	{
		float newMag = Vec3(Matrix::Mul44x41(trans.matrix, direction * dist)).Magnitude();
		return (newMag < intersectDist) ? true : false;
	}

	Ray();
	Ray(Point3 start, Vec3 dir) :start(start), direction(dir) {}
	~Ray();
};



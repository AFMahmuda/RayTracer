#pragma once
#include"Geometry.h"
#include<iostream>
class Sphere : public Geometry
{


public:
	Point3 c; //center
	float r; //radius

	Sphere(float * val) :Sphere( Point3(val[0], val[1], val[2]), val[3]) {}
	Sphere() {}
	Sphere(Point3& center, float radius) {
		c = center;
		r = radius;
		trans.matrix = Matrix(4,4).Identity();
		hasMorton = false;
		updatePos();
		type = SPHERE;
	}

	bool isIntersecting(Ray& ray)
	{
		return true;
	}

	Vec3& getNormal(Point3& point)
	{
		Point3 p = Matrix::Mul44x41(trans.matrix.Inverse(), point);
		Vec3 * res = new Vec3(c, p);
		res->Normalize();
		return *res;
	}


	~Sphere();

	// Inherited via Geometry
	virtual void updatePos() override;
};


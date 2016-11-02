#pragma once
#include"Geometry.h"
#include<iostream>

class Sphere : public Geometry
{
public:
	Point3 c; //center
	float r; //radius

	Sphere(float * val) :Sphere(Point3(val[0], val[1], val[2]), val[3]) {}
	Sphere() {}
	Sphere(Point3& center, float radius);

	~Sphere();

	// Inherited via Geometry
	virtual void updatePos() override;
	virtual	bool isIntersecting(Ray& ray) override;
	virtual Vec3 getNormal(Point3& point) override;
};


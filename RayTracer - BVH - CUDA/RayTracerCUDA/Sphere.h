#pragma once
#include"Geometry.h"
#include<iostream>

class Sphere : public Geometry
{
public:
	Vec3 c; //center
	float r; //radius

	Sphere(float * val) :Sphere(Vec3(val[0], val[1], val[2], 1), val[3]) {}
	Sphere() {}
	Sphere(Vec3& center, float radius);

	~Sphere();

	// Inherited via Geometry
	virtual void updatePos() override;
	virtual	bool isIntersecting(Ray& ray) override;
	virtual Vec3 getNormal(Vec3& point) override;
};


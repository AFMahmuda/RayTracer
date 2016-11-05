#pragma once
#include"Geometry.h"
#include<iostream>

class Sphere : public Geometry
{
public:
	vec3 c; //center
	float r; //radius

	Sphere(float * val) :Sphere(vec3(val[0], val[1], val[2]), val[3]) {}
	Sphere() {}
	Sphere(vec3& center, float radius);

	~Sphere();

	// Inherited via Geometry
	virtual void updatePos() override;
	virtual	bool isIntersecting(Ray& ray) override;
	virtual vec3 getNormal(vec3& point) override;
};


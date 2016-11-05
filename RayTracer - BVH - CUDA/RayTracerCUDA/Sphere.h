#pragma once
#include"Geometry.h"
#include<iostream>

class Sphere : public Geometry
{
public:
	Data3D c; //center
	float r; //radius

	Sphere(float * val) :Sphere(Data3D(val[0], val[1], val[2]), val[3]) {}
	Sphere() {}
	Sphere(Data3D& center, float radius);

	~Sphere();

	// Inherited via Geometry
	virtual void updatePos() override;
	virtual	bool isIntersecting(Ray& ray) override;
	virtual Data3D getNormal(Data3D& point) override;
};


#pragma once
#include"Container.h"
#include"Sphere.h"
#include"Triangle.h"

#include<algorithm>
#ifndef M_PI
#define M_PI 3.14159265358979323846
#endif


class SphereContainer :
	public Container
{
public:
	Vec3 c;
	float r;

	SphereContainer(std::shared_ptr< Geometry> item);
	SphereContainer(std::shared_ptr<SphereContainer> a, std::shared_ptr<SphereContainer> b);
	~SphereContainer();

	// Inherited via Container
	virtual bool isIntersecting(Ray& ray) override;
	virtual void showInfo() override;
};


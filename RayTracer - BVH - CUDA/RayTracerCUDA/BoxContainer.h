#pragma once
#include "Container.h"
#include"Sphere.h"
#include"Triangle.h"

#include<algorithm>
class BoxContainer :
	public Container
{
public:
	vec3 min = vec3(INFINITY, INFINITY, INFINITY,1);
	vec3 max = vec3(-INFINITY, -INFINITY, -INFINITY,1);
	BoxContainer(std::shared_ptr<Geometry> item);
	void setMinMax(vec3* points, int n);
	BoxContainer(std::shared_ptr<BoxContainer>& a, std::shared_ptr<BoxContainer>& b);

	BoxContainer();
	~BoxContainer();

	// Inherited via Container
	virtual bool isIntersecting(Ray& ray) override;
	virtual void showInfo() override;
};


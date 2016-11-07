#pragma once
#include "Container.h"
#include"Triangle.h"

#include<algorithm>
class BoxContainer :
	public Container
{
public:
	Vec3 min = Vec3(INFINITY, INFINITY, INFINITY,1);
	Vec3 max = Vec3(-INFINITY, -INFINITY, -INFINITY,1);
	BoxContainer(std::shared_ptr<Triangle> item);
	void setMinMax(Vec3* points, int n);
	BoxContainer(std::shared_ptr<BoxContainer>& a, std::shared_ptr<BoxContainer>& b);

	BoxContainer();
	~BoxContainer();

	// Inherited via Container
	virtual bool isIntersecting(Ray& ray) override;
	virtual void showInfo() override;
};


#pragma once
#include "Container.h"
#include"Sphere.h"
#include"Triangle.h"

#include<algorithm>
class BoxContainer :
	public Container
{
public:
	Data3D min = Data3D(INFINITY, INFINITY, INFINITY,1);
	Data3D max = Data3D(-INFINITY, -INFINITY, -INFINITY,1);
	BoxContainer(std::shared_ptr<Geometry> item);
	void setMinMax(Data3D* points, int n);
	BoxContainer(std::shared_ptr<BoxContainer>& a, std::shared_ptr<BoxContainer>& b);

	BoxContainer();
	~BoxContainer();

	// Inherited via Container
	virtual bool isIntersecting(Ray& ray) override;
	virtual void showInfo() override;
};


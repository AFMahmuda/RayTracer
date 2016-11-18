#pragma once
#include"Container.h"
#include"Triangle.h"


class SphereContainer :
	public Container
{
public:
	Vec3 c;
	float r;

	SphereContainer(std::shared_ptr< Triangle> item);
	SphereContainer(std::shared_ptr<SphereContainer> a, std::shared_ptr<SphereContainer> b);
	~SphereContainer();

	// Inherited via Container
	virtual bool isIntersecting(Ray& ray) override;
	virtual void showInfo() override;
};


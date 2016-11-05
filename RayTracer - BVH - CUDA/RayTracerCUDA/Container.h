#pragma once
#include<vector>
#include<memory>

#include"Vec3.h"
#include"Geometry.h"
#include"Ray.h"
class Container
{
public:
	enum TYPE {
		BOX, SPHERE
	};
	TYPE type;
	bool isLeaf = false;
	std::shared_ptr<Container> lChild;
	std::shared_ptr<Container> rChild;
	std::shared_ptr<Geometry> geo;
	float area;

	virtual bool isIntersecting(Ray& ray) = 0;
	virtual void showInfo() = 0;

	float areaWithClosest = INFINITY;
	std::shared_ptr<Container> closest = nullptr;


	Container();
	~Container();
};


#pragma once
#include<vector>
#include<memory>

#include"Ray.h"
class Geometry;//forward declaration
class Container
{
public:
	enum TYPE {
		BOX, SPHERE
	};
	TYPE type;
	bool isLeaf = false;
	std::shared_ptr<Container> LChild;
	std::shared_ptr<Container> RChild;
	std::shared_ptr<Geometry> geo;
	float area;

	virtual bool IsIntersecting(Ray& ray) = 0;
	virtual void showInfo() = 0;

	float areaWithClosest = INFINITY;
	std::shared_ptr<Container> closest = nullptr;


	Container();
	~Container();
};


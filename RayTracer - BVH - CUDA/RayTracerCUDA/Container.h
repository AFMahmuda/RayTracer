#pragma once
#include<vector>
#include"Geometry.h"

class Container
{
public:
	enum TYPE {
		BOX, SPHERE
	};
	TYPE type;
	bool isLeaf = false;
	Container* LChild;
	Container* RChild;
	Geometry* geo;
	float area;

	virtual bool IsIntersecting(Ray ray) = 0;
	virtual void showInfo() = 0;

	float areaWithClosest = INFINITY;
	Container* closest = nullptr;


	Container();
	~Container();
};


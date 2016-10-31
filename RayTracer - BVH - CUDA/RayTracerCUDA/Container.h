#pragma once
#include<vector>
#include"Geometry.h"
#include<memory>
using namespace std;
class Container
{
public:
	enum TYPE {
		BOX, SPHERE
	};
	TYPE type;
	bool isLeaf = false;
	shared_ptr<Container> LChild;
	shared_ptr<Container> RChild;
	shared_ptr<Geometry> geo;
	float area;

	virtual bool IsIntersecting(Ray ray) = 0;
	virtual void showInfo() = 0;

	float areaWithClosest = INFINITY;
	shared_ptr<Container> closest = nullptr;


	Container();
	~Container();
};


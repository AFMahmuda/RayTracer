#pragma once
#include "Transform.h"
class Translation :
	public Transform
{
public:
	Translation() :Translation(0, 0, 0) {}
	Translation(float * vals) : Translation(vals[0], vals[1], vals[2]) {}
	Translation(float x, float y, float z);

	~Translation();
};


#pragma once
#include "Transform.h"
class Scaling :
	public Transform
{
public:
	Scaling() :Scaling(1, 1, 1) {}
	Scaling(float * vals): Scaling(vals[0],vals[1],vals[2]){}
	Scaling(float x, float y, float z);

	~Scaling();
};


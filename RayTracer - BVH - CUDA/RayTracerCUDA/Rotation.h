#pragma once
#include "Transform.h"


class Rotation :
	public Transform
{
public:
	Rotation() : Rotation(new float[3]{ 0,0,0 }, 0) {}
	Rotation(float* xyz, float deg);
	Rotation(float * values) :Rotation(new float[3]{ values[0] ,values[1] ,values[2] },values[3]) {}

	~Rotation();
};


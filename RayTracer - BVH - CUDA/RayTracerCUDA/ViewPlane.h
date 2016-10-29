#pragma once
#include"Point3.h"
#include "Camera.h"
//#define _USE_MATH_DEFINES
#include <cmath>
#ifndef M_PI
#define M_PI 3.14159265358979323846
#endif

class ViewPlane
{
public:


	static ViewPlane& Instance();

	float worldH;
	float worldW;
	int pixelH;
	int pixelW;
	Point3 pos;
	Point3 upperLeft;
	Point3 unitRight;
	Point3 unitDown;

	void Init(int width, int height);

	void Precalculate();

	Point3 &getUpperLeft();

	Point3 &getNewLocation(int col, int row);
protected:

	ViewPlane(); // Prevent construction
	ViewPlane(const ViewPlane&); // Prevent construction by copying
	ViewPlane& operator=(const ViewPlane&); // Prevent assignment
	~ViewPlane(); // Prevent unwanted destruction
};


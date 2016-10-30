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

	static bool flag;
	static ViewPlane* instance;

public:

	static ViewPlane* Instance()
	{
		if (flag == false)
		{
			instance = new ViewPlane();
			flag = true;
		}
		return instance;
	}

	float worldH = 1;
	float worldW = 1;
	int pixelH = 1;
	int pixelW = 1;
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
//	ViewPlane& operator=(const ViewPlane&); // Prevent assignment
	~ViewPlane(); // Prevent unwanted destruction
};


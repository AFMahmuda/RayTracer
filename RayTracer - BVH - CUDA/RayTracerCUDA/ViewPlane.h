#pragma once
#include "Vec3.h"
#include "Camera.h"



class ViewPlane
{



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

	void Init(int width, int height);

	vec3 getNewLocation(int col, int row);
private:
	static bool flag;
	static ViewPlane* instance;

	vec3 pos;
	vec3 upperLeft;
	vec3 unitRight;
	vec3 unitDown;


	void Precalculate();

	vec3 &getUpperLeft();

	ViewPlane(); // Prevent construction
	ViewPlane(const ViewPlane&); // Prevent construction by copying
//	ViewPlane& operator=(const ViewPlane&); // Prevent assignment
	~ViewPlane(); // Prevent unwanted destruction
};


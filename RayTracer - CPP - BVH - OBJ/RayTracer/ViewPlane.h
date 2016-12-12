#pragma once
#include "Vec3.h"
#include "Camera.h"

class ViewPlane
{
public:

	static ViewPlane* getInstance()
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

	void init(int width, int height);
	Vec3 getNewLocation(int col, int row);
private:
	static bool flag;
	static ViewPlane* instance;

	Vec3 pos;
	Vec3 upperLeft;
	Vec3 unitRight;
	Vec3 unitDown;


	void precalculate();
	void setUpperleft();

	ViewPlane(); // Prevent construction
	ViewPlane(const ViewPlane&); // Prevent construction by copying
	~ViewPlane(); // Prevent unwanted destruction
};


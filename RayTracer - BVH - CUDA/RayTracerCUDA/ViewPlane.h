#pragma once
#include"Data3D.h"
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

	Data3D getNewLocation(int col, int row);
private:
	static bool flag;
	static ViewPlane* instance;

	Data3D pos;
	Data3D upperLeft;
	Data3D unitRight;
	Data3D unitDown;


	void Precalculate();

	Data3D &getUpperLeft();

	ViewPlane(); // Prevent construction
	ViewPlane(const ViewPlane&); // Prevent construction by copying
//	ViewPlane& operator=(const ViewPlane&); // Prevent assignment
	~ViewPlane(); // Prevent unwanted destruction
};


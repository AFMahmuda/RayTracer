#pragma once
#include"IData3D.h"

class Point3 : public Data3D {
protected:


public:

	Point3(float x, float y, float z)
	{
		Point3::x = x;
		Point3::y = y;
		Point3::z = z;
		h = 1;
	}
	Point3() : Point3(0, 0, 0) {}
	Point3(float* params) :Point3(params[0], params[1], params[2]) {}
	Point3(const Data3D& vec) :Point3(vec[0], vec[1], vec[2]) {}

	~Point3();


};




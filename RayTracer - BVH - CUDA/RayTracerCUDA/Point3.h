#pragma once
#include"Data3D.h"

class Point3 : public Data3D {
protected:


public:

	Point3(float x, float y, float z)
	{

		v[0] = x;
		v[1] = y;
		v[2] = z;
		v[3] = 0;
	}
	Point3() : Point3(0, 0, 0) {}
	Point3(float* params) :Point3(params[0], params[1], params[2]) {}
	Point3(const Data3D& vec) :Point3(vec[0], vec[1], vec[2]) {}

	~Point3();


};




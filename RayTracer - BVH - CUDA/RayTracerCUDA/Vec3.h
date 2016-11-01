#pragma once
#include"IData3D.h"

#include <math.h>
class Vec3 : public Data3D{
public:	
	Vec3(float x, float y, float z)
	{
		this->x = x;
		this->y = y;
		this->z = z;
		h = 0;
	}
	Vec3() : Vec3(0, 0, 0) {}
	Vec3(float * params);
	Vec3(const Data3D& origin);

	Vec3(const Data3D& start,const Data3D& end);

	float Magnitude();
	Vec3& Normalize();

	float operator*(const Vec3 & b)
	{
		return x*b.x + y*b.y + z*b.z;
	}
	
	static Vec3 Cross(const Vec3 & a, const Vec3 & b);
	Vec3 Cross(const Vec3 & b)
	{
		float X, Y, Z;
		X = (y * b.z) - (z * b.y);
		Y = ((x * b.z) - (z * b.x)) * -1;
		Z = (x * b.y) - (y * b.x);

		Vec3& res = Vec3(X, Y, Z);
		return res;
	}
	~Vec3();
};


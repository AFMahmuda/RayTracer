#pragma once
#include"Data3D.h"

#include <math.h>
class Vec3 : public Data3D {
public:
	Vec3(float x, float y, float z)
	{
		v[0] = x;
		v[1] = y;
		v[2] = z;
		v[3] = 0;
	}
	Vec3() : Vec3(0, 0, 0) {}
	Vec3(float * params);
	Vec3(const Data3D& origin);

	Vec3(const Data3D& start, const Data3D& end);

	float Magnitude();
	Vec3& Normalize();

	float operator*(const Vec3 & b)
	{
		return v[0] * b.v[0] + v[1] * b.v[1] + v[2] * b.v[2];
	}

	static Vec3 Cross(const Vec3 & a, const Vec3 & b);
	Vec3 Cross(const Vec3 & b)
	{
		float X, Y, Z;
		X = (v[1] * b.v[2]) - (v[2] * b.v[1]);
		Y = ((v[0] * b.v[2]) - (v[2] * b.v[0])) * -1;
		Z = (v[0] * b.v[1]) - (v[1] * b.v[0]);

		Vec3& res = Vec3(X, Y, Z);
		return res;
	}
	~Vec3();
};


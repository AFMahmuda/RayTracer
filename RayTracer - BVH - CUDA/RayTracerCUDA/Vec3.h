#pragma once
#include "Point3.h"
#include <math.h>
class Vec3 :
	public Point3
{
public:
	Vec3(float x, float y, float z);
	Vec3();
	Vec3(float * params);
	Vec3(Vec3& origin);
	Vec3(Point3& point);
	Vec3(Point3& start, Point3& end);


	float Magnitude();
	Vec3& Normalize();

	static Vec3* UP();
	static Vec3* DOWN();
	static Vec3* LEFT();
	static Vec3* RIGHT();

	float operator*(const Vec3 & b)
	{
		return x*b.x + y*b.y + z*b.z;
	}

	Vec3& operator+=(const Vec3& rhs)
	{
		x += rhs.x;
		y += rhs.y;
		z += rhs.z;
		return *this;
	}
	Vec3& operator-=(const Vec3& rhs)
	{
		x -= rhs.x;
		y -= rhs.y;
		z -= rhs.z;
		return *this;
	}

	
	Vec3& operator*=(float rhs)
	{
		x *= rhs;
		y *= rhs;
		z *= rhs;
		return *this;
	}


	friend Vec3 operator+(Vec3 lhs, const Vec3& rhs)
	{
		lhs += rhs;
		return lhs;
	}

	friend Vec3 operator-(Vec3 lhs, const Vec3& rhs)
	{
		lhs -= rhs;
		return lhs;
	}
	friend Vec3 operator*(Vec3 lhs, float rhs)
	{
		lhs *= rhs;
		return lhs;
	}
	friend Vec3 operator*(float lhs, Vec3 rhs)
	{
		rhs *= lhs;
		return rhs;
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


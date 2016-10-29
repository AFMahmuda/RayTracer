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

	Vec3& operator+(const Vec3 & b) {
		return  Vec3(x + b.x, y + b.y, z + b.z);
	}

	Vec3& operator-(const Vec3 & b) {
		return  Vec3(x - b.x, y - b.y, z - b.z);
	}


	Vec3& operator*(float scalar) {
		return  Vec3(x*scalar, y*scalar, z*scalar);
	}

	Vec3& operator/(float scalar) {
		scalar = 1.f / scalar;
		return  Vec3(x*scalar, y*scalar, z*scalar);
	}
	static Vec3& Cross(const Vec3 & a, const Vec3 & b) {
		return  Vec3(
			a.y * b.z - a.z * b.y,
			a.z * b.x - a.x * b.z,
			a.x * b.y - a.y * b.x
			);
	}

	~Vec3();
};


#pragma once

class Vec3
{

public:
	int n = 3;
	float* v; //values

	Vec3(float * vals);
	Vec3(float x, float y, float z, float h = 0);
	Vec3(float * vals, float h);
	Vec3(Vec3 a, Vec3 b);
	Vec3(const Vec3& ori);
	float& operator[](int index)
	{
		return v[index];
	}
	const float& operator[](int index) const
	{
		return v[index];
	}
	float magnitude();
	Vec3 normalize();

	Vec3& operator+=(const Vec3& rhs);
	Vec3& operator-=(const Vec3& rhs);
	Vec3& operator*=(float rhs);
	Vec3& operator=(const Vec3& other);

	float operator*(const Vec3 & b)
	{
		return v[0] * b.v[0] + v[1] * b.v[1] + v[2] * b.v[2];
	}
	static Vec3 Cross(const Vec3 & a, const Vec3 & b);

	friend Vec3 operator+(const Vec3& lhs, const Vec3& rhs)
	{
		Vec3 res(lhs.v);
		res += rhs;
		return res;
	}

	friend Vec3 operator-(const Vec3& lhs, const Vec3& rhs)
	{
		Vec3 res(lhs.v);
		res -= rhs;
		return res;
	}
	friend Vec3 operator*(const Vec3& lhs, float rhs)
	{
		Vec3 res(lhs.v);
		res *= rhs;
		return res;
	}
	friend Vec3 operator*(float lhs, const Vec3& rhs)
	{
		return rhs * lhs;
	}

	void show();

	Vec3();
	~Vec3();
};


#pragma once
#include<iostream>	


class vec3
{

public:
	int n = 3;
	float* v; //values

	vec3(float * vals);
	vec3(float * vals, float h) : vec3(new float[4]{ vals[0], vals[1], vals[2], h }) {}
	vec3(float x, float y, float z, float h = 0) :vec3(new float[4]{ x, y, z, h }) {}
	vec3(vec3 a, vec3 b) :vec3(b - a) { v[3] = 0; }
	vec3(const vec3& ori) :vec3(ori.v) {}
	float& operator[](int index)
	{
		return v[index];
	}
	const float& operator[](int index) const
	{
		return v[index];
	}
	float Magnitude();
	vec3 Normalize();

	vec3& operator+=(const vec3& rhs);
	vec3& operator-=(const vec3& rhs);
	vec3& operator*=(float rhs);


	float operator*(const vec3 & b)
	{
		return v[0] * b.v[0] + v[1] * b.v[1] + v[2] * b.v[2];
	}

	static vec3 Cross(const vec3 & a, const vec3 & b);
	friend vec3 operator+(const vec3 lhs, const vec3& rhs)
	{
		vec3 res(lhs.v);
		res += rhs;
		return res;
	}

	friend vec3 operator-(const vec3 lhs, const vec3& rhs)
	{
		vec3 res(lhs.v);
		res -= rhs;
		return res;
	}
	friend vec3 operator*(const vec3 lhs, float rhs)
	{
		vec3 res(lhs.v);
		res *= rhs;
		return res;
	}
	friend vec3 operator*(float lhs, const vec3 rhs)
	{
		return rhs * lhs;
	}

	vec3() {
		v = new float[4];
	}

	void show() {
		std::cout << v[0] << " " << v[1] << " " << v[2];
	}


	~vec3() {
	}
};


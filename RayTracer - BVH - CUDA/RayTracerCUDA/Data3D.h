#pragma once
#include<iostream>	


class Data3D
{

public:
	int n = 3;
	float* v; //values

	Data3D(float * vals);
	Data3D(float * vals, float h) : Data3D(new float[4]{ vals[0], vals[1], vals[2], h }) {}
	Data3D(float x, float y, float z, float h = 0) :Data3D(new float[4]{ x, y, z, h }) {}
	Data3D(Data3D a, Data3D b) :Data3D(b - a) { v[3] = 0; }
	Data3D(Data3D& ori) :Data3D(ori.v) {}
	Data3D(const Data3D& ori) :Data3D(ori.v) {}
	float& operator[](int index)
	{
		return v[index];
	}
	const float& operator[](int index) const
	{
		return v[index];
	}
	float Magnitude();
	Data3D Normalize();

	Data3D& operator+=(const Data3D& rhs);
	Data3D& operator-=(const Data3D& rhs);
	Data3D& operator*=(float rhs);


	float operator*(const Data3D & b)
	{
		return v[0] * b.v[0] + v[1] * b.v[1] + v[2] * b.v[2];
	}

	static Data3D Cross(const Data3D & a, const Data3D & b);
	friend Data3D operator+(const Data3D lhs, const Data3D& rhs)
	{
		Data3D res(lhs.v);
		res += rhs;
		return res;
	}

	friend Data3D operator-(const Data3D lhs, const Data3D& rhs)
	{
		Data3D res(lhs.v);
		res -= rhs;
		return res;
	}
	friend Data3D operator*(const Data3D lhs, float rhs)
	{
		Data3D res(lhs.v);
		res *= rhs;
		return res;
	}
	friend Data3D operator*(float lhs, const Data3D rhs)
	{
		return rhs * lhs;
	}

	Data3D() {
		v = new float[4];
	}

	void show() {
		std::cout << v[0] << " " << v[1] << " " << v[2];
	}


	~Data3D() {
	}
};


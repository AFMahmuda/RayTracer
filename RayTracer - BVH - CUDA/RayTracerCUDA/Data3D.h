#pragma once


class Data3D
{

public:
	int n = 3;
	float* v; //values
	float& operator[](int index)
	{
		return v[index];
	}
	const float& operator[](int index) const
	{
		return v[index];
	}

	Data3D& operator+=(const Data3D& rhs);
	Data3D& operator-=(const Data3D& rhs);
	Data3D& operator*=(float rhs);

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
	Data3D(float * vals);
	~Data3D() {
	}
};


#pragma once


class Data3D
{

public:
	int n = 4;
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
	Data3D& operator-=(const Data3D& rhs)
	{
		//v[0] -= rhs.v[0];
		//v[1] -= rhs.v[1];
		//v[2] -= rhs.v[2];

		return *this;
	}
	Data3D& operator*=(float rhs)
	{
		v[0] *= rhs;
		v[1] *= rhs;
		v[2] *= rhs;
		return *this;
	}


	friend Data3D operator+(Data3D lhs, const Data3D& rhs)
	{
		Data3D res(lhs);
		res += rhs;
		return res;
	}

	friend Data3D operator-(Data3D lhs, const Data3D& rhs)
	{
		Data3D res(lhs);
		res -= rhs;
		return res;
	}
	friend Data3D operator*(Data3D lhs, float rhs)
	{
		Data3D res(lhs);
		res *= rhs;
		return res;
	}
	friend Data3D operator*(float lhs, Data3D rhs)
	{
		Data3D res(rhs);
		res *= lhs;
		return res;
	}

	Data3D() {
		v = new float[n];
	}
	~Data3D() {
	}
};


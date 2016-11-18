#pragma once
#include<cmath>
class MyColor
{
public:
	MyColor() :MyColor(0, 0, 0) {}
	MyColor(float r, float g, float b);

	float r, g, b;
	void setR(float val);
	void setG(float val);
	void setB(float val);

	MyColor& operator=(const MyColor& other) {
		setR(other.r);
		setG(other.g);
		setB(other.b);
		return *this;

	}

	MyColor& operator+=(const MyColor& rhs)
	{
		setR(r + rhs.r);
		setG(g + rhs.g);
		setB(b + rhs.b);
		return *this;
	}
	MyColor& operator-=(const MyColor& rhs)
	{
		setR(r - rhs.r);
		setG(g - rhs.g);
		setB(b - rhs.b);
		return *this;
	}
	MyColor& operator*=(float rhs)
	{
		setR(r * rhs);
		setG(g * rhs);
		setB(b * rhs);
		return *this;
	}

	MyColor& operator*=(MyColor rhs)
	{
		setR(r * rhs.r);
		setG(g * rhs.g);
		setB(b * rhs.b);
		return *this;
	}


	friend MyColor operator+(MyColor lhs, const MyColor& rhs)
	{
		lhs += rhs;
		return lhs;
	}

	friend MyColor operator-(MyColor lhs, const MyColor& rhs)
	{
		lhs -= rhs;
		return lhs;
	}

	friend MyColor operator*(MyColor lhs, const MyColor& rhs)
	{
		lhs *= rhs;
		return lhs;
	}

	friend MyColor operator*(MyColor lhs, float rhs)
	{
		lhs *= rhs;
		return lhs;
	}
	friend MyColor operator*(float lhs, MyColor rhs)
	{
		rhs *= lhs;
		return rhs;
	}



	~MyColor();
};


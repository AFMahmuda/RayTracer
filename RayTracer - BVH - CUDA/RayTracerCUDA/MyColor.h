#pragma once
#include<cmath>
class MyColor
{
public:
	MyColor();
	MyColor(float r, float g, float b);

	float r, g, b;
	void setR(float val);
	void setG(float val);
	void setB(float val);

	MyColor & operator+(MyColor& other)
	{
		return MyColor(r + other.r, g + other.g, b + other.b);
	}
	MyColor & operator-(MyColor& other)
	{
		return MyColor(r - other.r, g - other.g, b - other.b);
	}

	MyColor & operator*(MyColor& other)
	{
		return MyColor(r * other.r, g * other.g, b * other.b);
	}

	MyColor & operator*(float s)
	{
		return MyColor(r * s, g * s, b * s);
	}


	MyColor& Pow(float p)
	{
		return MyColor(
			powf(r, p),
			powf(g, p),
			powf(b, p)
			);

	}



	~MyColor();
};


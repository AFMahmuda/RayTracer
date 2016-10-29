#pragma once
class Point3
{
protected:


public:
	float x = 0, y = 0, z = 0;
	float h = 1;


	Point3(float x, float y, float z);
	Point3() : Point3(0, 0, 0) {}
	Point3(float* params) :Point3(params[0], params[1], params[2]) {}

	static Point3 ZERO();

	~Point3();

	//float getX();
	//float getY();
	//float getZ();

	//	void setX(float val) { x = val; }
		//void setY(float val) { y = val; }
		//void setZ(float val) { z = val; }



	float& operator[](int index)
	{
		{
			switch (index)
			{
			case 0:
				return x;
				break;
			case 1:
				return y;
				break;
			case 2:
				return z;
				break;
			case 3:
				return h;
				break;
			}
		}
	}

	Point3& operator+=(const Point3& rhs)
	{
		x += rhs.x;
		y += rhs.y;
		z += rhs.z;
		return *this;
	}
	Point3& operator-=(const Point3& rhs)
	{
		x -= rhs.x;
		y -= rhs.y;
		z -= rhs.z;
		return *this;
	}

	Point3& operator*=(const Point3& rhs)
	{
		x *= rhs.x;
		y *= rhs.y;
		z *= rhs.z;
		return *this;
	}
	Point3& operator*=(float rhs)
	{
		x *= rhs;
		y *= rhs;
		z *= rhs;
		return *this;
	}


	friend Point3 operator+(Point3 lhs, const Point3& rhs)
	{
		lhs += rhs;
		return lhs;
	}
	friend Point3 operator*(Point3 lhs, const Point3& rhs)
	{
		lhs *= rhs;
		return lhs;
	}
	friend Point3 operator-(Point3 lhs, const Point3& rhs)
	{
		lhs -= rhs;
		return lhs;
	}
	friend Point3 operator*(Point3 lhs, float rhs)
	{
		lhs *= rhs;
		return lhs;
	}
	friend Point3 operator*(float lhs, Point3 rhs)
	{
		rhs *= lhs;
		return rhs;
	}


};




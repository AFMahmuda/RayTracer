#pragma once
class Data3D
{
public:
	float x = 0, y = 0, z = 0;
	float h = 1;
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
	const float& operator[](int index) const
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

	Data3D& operator+=(const Data3D& rhs)
	{
		x += rhs.x;
		y += rhs.y;
		z += rhs.z;
		return *this;
	}
	Data3D& operator-=(const Data3D& rhs)
	{
		x -= rhs.x;
		y -= rhs.y;
		z -= rhs.z;
		return *this;
	}
	Data3D& operator*=(float rhs)
	{
		x *= rhs;
		y *= rhs;
		z *= rhs;
		return *this;
	}


	friend Data3D operator+(Data3D lhs, const Data3D& rhs)
	{
		lhs += rhs;
		return lhs;
	}

	friend Data3D operator-(Data3D lhs, const Data3D& rhs)
	{
		lhs -= rhs;
		return lhs;
	}
	friend Data3D operator*(Data3D lhs, float rhs)
	{
		lhs *= rhs;
		return lhs;
	}
	friend Data3D operator*(float lhs, Data3D rhs)
	{
		rhs *= lhs;
		return rhs;
	}

	Data3D();
	~Data3D();
};


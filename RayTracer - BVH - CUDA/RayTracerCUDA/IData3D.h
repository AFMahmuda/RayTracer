#pragma once
class IData3D
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

	IData3D& operator+=(const IData3D& rhs)
	{
		x += rhs.x;
		y += rhs.y;
		z += rhs.z;
		return *this;
	}
	IData3D& operator-=(const IData3D& rhs)
	{
		x -= rhs.x;
		y -= rhs.y;
		z -= rhs.z;
		return *this;
	}
	IData3D& operator*=(float rhs)
	{
		x *= rhs;
		y *= rhs;
		z *= rhs;
		return *this;
	}


	friend IData3D operator+(IData3D lhs, const IData3D& rhs)
	{
		lhs += rhs;
		return lhs;
	}

	friend IData3D operator-(IData3D lhs, const IData3D& rhs)
	{
		lhs -= rhs;
		return lhs;
	}
	friend IData3D operator*(IData3D lhs, float rhs)
	{
		lhs *= rhs;
		return lhs;
	}
	friend IData3D operator*(float lhs, IData3D rhs)
	{
		rhs *= lhs;
		return rhs;
	}

	IData3D();
	~IData3D();
};


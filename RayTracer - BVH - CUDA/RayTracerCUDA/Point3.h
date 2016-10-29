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

	//const float operator[](int index) const {
	//	return operator[](index);
	//}

	Point3& operator+(const Point3& b) {
		return  Point3(x + b.x, y + b.y, z + b.z);
	}

	Point3& operator-(const Point3& b) {
		return  Point3(x - b.x, y - b.y, z - b.z);
	}
	Point3& operator*(float s) {
		return  Point3(x * s, y *s, z *s);
	}
	Point3& operator/(float s) {
		return  Point3(x / s, y / s, z / s);
	}





};




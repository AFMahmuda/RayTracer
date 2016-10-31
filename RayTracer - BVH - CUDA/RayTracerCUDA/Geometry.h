#pragma once
#include"Point3.h"
#include"Transform.h"
#include"Material.h"
#include"Translation.h"
#include"Matrix.h"
#include"Ray.h"
#include<bitset>
#include<string>
#include<iostream>


class Geometry
{

protected:
	bool hasMorton = false;
	unsigned int mortonCode;
	Transform trans;
	Point3 pos;

public:

	enum  TYPE
	{
		SPHERE,
		TRIANGLE
	};

	TYPE type;

	Transform& getTrans()
	{
		return trans;
	}
	void setTrans(const Transform& trans)
	{
		Geometry::trans.matrix = trans.matrix;
		hasMorton = false;
		updatePos();
		return;
	}

	Material mat;
	MyColor ambient;

	virtual void updatePos() = 0;
	virtual bool isIntersecting(Ray& ray) = 0;
	virtual Vec3 &getNormal(Point3& point) = 0;


	unsigned int getMortonPos()
	{
		if (!hasMorton)
		{
			unsigned int x = expandBits(fminf(fmaxf(pos[0] * 1024.f, 0.f), 1023.f));
			unsigned int y = expandBits(fminf(fmaxf(pos[1] * 1024.f, 0.f), 1023.f));
			unsigned int z = expandBits(fminf(fmaxf(pos[2] * 1024.f, 0.f), 1023.f));
			mortonCode = x * 4 + y * 2 + z;
			hasMorton = true;
		}
		return mortonCode;
	}

	unsigned int expandBits(unsigned int v)
	{
		v = (v * 0x00010001u) & 0xFF0000FFu;
		v = (v * 0x00000101u) & 0x0F00F00Fu;
		v = (v * 0x00000011u) & 0xC30C30C3u;
		v = (v * 0x00000005u) & 0x49249249u;
		return v;
	}

	std::string getMortonBitString()
	{
		return std::bitset<30>(getMortonPos()).to_string();
	}

	Geometry();

	~Geometry();
};


#pragma once
#include<bitset>
#include<string>
#include<algorithm>
#include<iostream>


#include "Vec3.h"
#include"Transform.h"
#include"Material.h"
#include"Translation.h"
#include"Matrix.h"
#include"Ray.h"

class Geometry
{

protected:
	bool hasMorton = false;
	unsigned int mortonCode;
	Transform trans;
	Vec3 pos;

public:

	enum TYPE { SPHERE, TRIANGLE };
	TYPE type;

	Transform& getTrans() { return trans; }
	void setTrans(const Transform& trans);

	Material mat;
	MyColor ambient;

	virtual void updatePos() = 0;
	virtual bool isIntersecting(Ray& ray) = 0;
	virtual Vec3 getNormal(Vec3& point) = 0;


	unsigned int getMortonPos();

	unsigned int expandBits(unsigned int v);

	std::string getMortonBitString();

	Geometry();
	~Geometry();
};


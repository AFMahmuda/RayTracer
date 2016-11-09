#pragma once
#include <string>
#include<bitset>

#include "Material.h"
#include "Vec3.h"
#include "Ray.h"

class Triangle
{
private:
	bool hasMorton = false;
	std::bitset<30> mortonBitset;
	unsigned int mortonCode;

	Vec3 localNorm;

	Vec3 ab;
	Vec3 ac;

	//dot products
	float d_ab_ab;
	float d_ab_ac;
	float d_ac_ac;
	float d_ab_ap;
	float d_ac_ap;

	float invDenom;
	void preCalculate();
	bool IsInsideTriangle(Vec3 point);
public:
	Vec3 pos;
	Vec3 a;
	Vec3 b;
	Vec3 c;
	Triangle(Vec3 a, Vec3 b, Vec3 c);
	Triangle();
	~Triangle();

	Material mat;

	virtual void updatePos();
	virtual bool isIntersecting(Ray& ray);
	virtual Vec3 getNormal(Vec3& point);


	unsigned int getMortonPos();
	unsigned int expandBits(unsigned int v);

	std::bitset<30>  getMortonBits();
};


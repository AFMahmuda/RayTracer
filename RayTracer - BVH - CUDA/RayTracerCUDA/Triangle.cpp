#include "Triangle.h"
#include<bitset>

Triangle::Triangle(Vec3 a, Vec3 b, Vec3 c) : Triangle() {
	this->a = a;
	this->b = b;
	this->c = c;
	hasMorton = false;
	updatePos();
	preCalculate();
}

void Triangle::preCalculate()
{

	//for IsInsideTriangle
	ab = Vec3(a, b);
	ac = Vec3(a, c);

	//for IsIntersect
	localNorm = Vec3::Cross(ac, ab).normalize();

	d_ab_ab = (ab)* (ab);
	d_ab_ac = (ab)* (ac);
	d_ac_ac = (ac)* (ac);

	invDenom = 1.0f / (d_ab_ab * d_ac_ac - d_ab_ac * d_ab_ac);
}

bool Triangle::isIntersecting(Ray & ray)
{
	//parallel -> return false
	if (ray.direction * localNorm == 0)
		return false;
	/*
	relative to ray direction
	*/
	float distanceToPlane = (
		(a * localNorm) -
		(ray.start * localNorm))
		/ (ray.direction * localNorm);
	/*
	dist < 0 = behind cam
	*/
	if (distanceToPlane > 0)
		if (ray.isCloser(distanceToPlane))
			if (IsInsideTriangle(ray.start + (ray.direction * distanceToPlane)))
			{
				ray.intersectDist = (ray.direction * distanceToPlane).magnitude();
				return true;
			}
	return false;
}

bool Triangle::IsInsideTriangle(Vec3 point) {
	Vec3 ap(point - a);

	d_ab_ap = ab * ap;
	d_ac_ap = ac * ap;

	float u = (d_ac_ac * d_ab_ap - d_ab_ac * d_ac_ap) * invDenom;
	float v = (d_ab_ab * d_ac_ap - d_ab_ac * d_ab_ap) * invDenom;

	return (u >= 0) && (v >= 0) && (u + v <= 1);
}

Vec3 Triangle::getNormal(Vec3 & point)
{
	Vec3  res = localNorm;
	res = res.normalize();
	return res;
}

Triangle::Triangle() :mat(Material())
{
}




Triangle::~Triangle()
{
}

void Triangle::updatePos()
{
	Vec3 temp = (a + b + c)* (.33f);
	pos[0] = (temp[0] / 100.f + .5f);
	pos[1] = (temp[1] / 100.f + .5f);
	pos[2] = (temp[2] / 100.f + .5f);

	getMortonPos();
}

unsigned int Triangle::getMortonPos()
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

unsigned int Triangle::expandBits(unsigned int v)
{
	v = (v * 0x00010001u) & 0xFF0000FFu;
	v = (v * 0x00000101u) & 0x0F00F00Fu;
	v = (v * 0x00000011u) & 0xC30C30C3u;
	v = (v * 0x00000005u) & 0x49249249u;
	return v;
}

std::string Triangle::getMortonBitString()
{
	return std::bitset<30>(getMortonPos()).to_string();
}
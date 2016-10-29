#pragma once
#include"Geometry.h"
class Triangle : public Geometry
{
public:
	Point3 a;
	Point3 b;
	Point3 c;

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

	Triangle(Point3 a, Point3 b, Point3 c) {
		this->a = a;
		this->b = b;
		this->c = c;
		trans.matrix = Matrix(4, 4).Identity();
		hasMorton = false;
		updatePos();
		type = TRIANGLE;
		preCalculate();
	}

	void preCalculate()
	{

		//for IsInsideTriangle
		ab = Vec3(a, b);
		ac = Vec3(a, c);

		//for IsIntersect
		localNorm = Vec3::Cross(ac, ab);
		localNorm.Normalize();

		d_ab_ab = (ab)* (ab);
		d_ab_ac = (ab)* (ac);
		d_ac_ac = (ac)* (ac);

		invDenom = 1.0f / (d_ab_ab * d_ac_ac - d_ab_ac * d_ab_ac);
	}

	bool isIntersecting(Ray & ray)
	{
		return true;
	}

	bool IsInsideTriangle(Point3 point)
	{
		Vec3 ap(point - a);

		d_ab_ap = ab * ap;
		d_ac_ap = ac * ap;

		float u = (d_ac_ac * d_ab_ap - d_ab_ac * d_ac_ap) * invDenom;
		float v = (d_ab_ab * d_ac_ap - d_ab_ac * d_ab_ap) * invDenom;

		return (u >= 0) && (v >= 0) && (u + v <= 1);
	}

	Vec3& getNormal(Point3 &point)
	{
		Vec3  res = (Matrix::Mul44x41(trans.matrix.Inverse(), localNorm));
		res.Normalize();
		return res;
	}

	void updatePos()
	{
		Point3 temp = (a + b + c)* (.33f);
		pos = Matrix::Mul44x41(trans.matrix, temp);
		pos[1] = (pos[0] / 100.f + .5f);
		pos[1] = (pos[1] / 100.f + .5f);
		pos[1] = (pos[1] / 100.f + .5f);

		getMortonPos();
	}

	Triangle();


	~Triangle();
};

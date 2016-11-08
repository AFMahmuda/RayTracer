#include "Triangle.h"



Triangle::Triangle(Vec3 a, Vec3 b, Vec3 c) {
	this->a = a;
	this->b = b;
	this->c = c;
	trans.matrix = Matrix(4, 4).Identity();
	hasMorton = false;
	updatePos();
	type = TRIANGLE;
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
		if (ray.isCloser(distanceToPlane, trans))
			if (IsInsideTriangle(ray.start + (ray.direction * distanceToPlane)))
			{
				ray.intersectDist = Matrix::Mul44x41(trans.matrix, ray.direction * distanceToPlane).magnitude();
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
	Vec3  res = (Matrix::Mul44x41(Matrix(trans.matrix.Inverse()), localNorm));
	res = res.normalize();
	return res;
}

Triangle::Triangle()
{
}




Triangle::~Triangle()
{
}

void Triangle::updatePos()
{
	Vec3 temp = (a + b + c)* (.33f);
	pos = Matrix::Mul44x41(trans.matrix, temp);
	pos[1] = (pos[0] / 100.f + .5f);
	pos[1] = (pos[1] / 100.f + .5f);
	pos[1] = (pos[1] / 100.f + .5f);

	getMortonPos();
}

#pragma once
#include"Geometry.h"
class Triangle : public Geometry
{
private:
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
	bool IsInsideTriangle(Point3 point);
public:
	Point3 a;
	Point3 b;
	Point3 c;
	Triangle(Point3 a, Point3 b, Point3 c);
	Triangle();
	~Triangle();

	// Inherited via Geometry
	virtual void updatePos() override;
	virtual	bool isIntersecting(Ray & ray) override;
	virtual Vec3 getNormal(Point3 &point) override;
};


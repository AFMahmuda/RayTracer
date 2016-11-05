#pragma once
#include"Geometry.h"
class Triangle : public Geometry
{
private:
	vec3 localNorm;

	vec3 ab;
	vec3 ac;

	//dot products
	float d_ab_ab;
	float d_ab_ac;
	float d_ac_ac;
	float d_ab_ap;
	float d_ac_ap;

	float invDenom;
	void preCalculate();
	bool IsInsideTriangle(vec3 point);
public:
	vec3 a;
	vec3 b;
	vec3 c;
	Triangle(vec3 a, vec3 b, vec3 c);
	Triangle();
	~Triangle();

	// Inherited via Geometry
	virtual void updatePos() override;
	virtual	bool isIntersecting(Ray & ray) override;
	virtual vec3 getNormal(vec3 &point) override;
};


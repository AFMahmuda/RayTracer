#pragma once
#include"Geometry.h"
class Triangle : public Geometry
{
private:
	Data3D localNorm;

	Data3D ab;
	Data3D ac;

	//dot products
	float d_ab_ab;
	float d_ab_ac;
	float d_ac_ac;
	float d_ab_ap;
	float d_ac_ap;

	float invDenom;
	void preCalculate();
	bool IsInsideTriangle(Data3D point);
public:
	Data3D a;
	Data3D b;
	Data3D c;
	Triangle(Data3D a, Data3D b, Data3D c);
	Triangle();
	~Triangle();

	// Inherited via Geometry
	virtual void updatePos() override;
	virtual	bool isIntersecting(Ray & ray) override;
	virtual Data3D getNormal(Data3D &point) override;
};


#pragma once
#include "Light.h"
class PointLight :
	public Light
{
public:
	Point3 * pos;
	PointLight();
	PointLight(Point3 * dir, MyColor * color) {
		this->color = color;
		this->pos = dir;
	}
	Vec3 * getPointToLight(Point3 * point) {
		return new Vec3(Point3(*point), Point3(*pos));
	}
	bool IsEffective(Point3 * point, Container * bvh) { return true; }
	float getAttValue(Point3 * point, Attenuation * att) {
		float d = getPointToLight(point)->Magnitude();
		return 1.f / (att->cons + (att-> line * d) + (att->line * d * d));
	}
	
	~PointLight();
};


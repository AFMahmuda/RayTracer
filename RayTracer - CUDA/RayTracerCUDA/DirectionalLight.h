#pragma once
#include "Light.h"
class DirectionalLight :
	public Light
{
public:
	Vec3 * dir;
	DirectionalLight();
	DirectionalLight(Vec3 * dir, MyColor * color) {
		this->color = color;
		this->dir = dir;
	}
	Vec3 * getPointToLight(Point3 * point) {
		return new Vec3(Vec3(*dir) * -1);
	}
	bool IsEffective(Point3 * point, Container * bvh) { return true; }
	float getAttValue(Point3 * point, Attenuation * att) {
		return 1;
	}


	~DirectionalLight();
};


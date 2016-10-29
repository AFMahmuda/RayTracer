#pragma once
#include"MyColor.h"
#include"Ray.h"
#include"Vec3.h"
#include"Point3.h"
#include"Container.h"
#include"Attenuation.h"
class Light
{
public:
	MyColor * color;
	virtual Vec3 * getPointToLight(Point3 * point) { return NULL; }
	virtual bool isEffective(Point3 * point, Container * bvh) { return false; }
	virtual float getAttValue(Point3 * point, Attenuation *  attenuation) { return 0; }
	Light();
	~Light();
};


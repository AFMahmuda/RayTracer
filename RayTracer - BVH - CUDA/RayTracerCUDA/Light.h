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
	virtual Vec3 getPointToLight(const Point3& point) = 0;
	virtual bool isEffective(Point3& point, Container& bvh) = 0;
	virtual float getAttValue(Point3& point, Attenuation&  attenuation) = 0;
	Light();
	~Light();
};


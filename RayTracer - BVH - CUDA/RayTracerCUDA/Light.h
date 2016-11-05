#pragma once
#include"MyColor.h"
#include"Ray.h"
#include"Container.h"
#include"Attenuation.h"
class Light
{
public:
	MyColor * color;
	virtual vec3 getPointToLight(const vec3& point) = 0;
	virtual bool isEffective(vec3& point, Container& bvh) = 0;
	virtual float getAttValue(vec3& point, Attenuation&  attenuation) = 0;
	Light();
	~Light();
};


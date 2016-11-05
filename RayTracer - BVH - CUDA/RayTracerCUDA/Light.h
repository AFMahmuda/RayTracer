#pragma once
#include"MyColor.h"
#include"Ray.h"
#include"Container.h"
#include"Attenuation.h"
class Light
{
public:
	MyColor * color;
	virtual Data3D getPointToLight(const Data3D& point) = 0;
	virtual bool isEffective(Data3D& point, Container& bvh) = 0;
	virtual float getAttValue(Data3D& point, Attenuation&  attenuation) = 0;
	Light();
	~Light();
};


#pragma once
#include"MyColor.h"
#include"Ray.h"
#include"Container.h"
#include"Attenuation.h"
class Light
{
public:
	MyColor * color;
	virtual Vec3 getPointToLight(const Vec3& point) = 0;
	virtual bool isEffective(Vec3& point, std::shared_ptr< Container> bvh) = 0;
	virtual float getAttValue(Vec3& point, Attenuation&  attenuation) = 0;
	Light();
	~Light();
};


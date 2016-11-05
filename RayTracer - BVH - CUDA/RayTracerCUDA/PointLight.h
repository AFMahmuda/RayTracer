#pragma once
#include "Light.h"
class PointLight :
	public Light
{
public:
	vec3 * pos;
	PointLight();
	PointLight(vec3 * dir, MyColor * color) {
		this->color = color;
		this->pos = dir;
	}

	~PointLight();

	// Inherited via Light
	virtual bool isEffective(vec3 & point, Container & bvh) override;
	virtual vec3 getPointToLight(const vec3 & point) override;
	virtual float getAttValue(vec3 & point, Attenuation & attenuation) override;
};


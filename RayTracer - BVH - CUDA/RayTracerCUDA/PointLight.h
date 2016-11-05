#pragma once
#include "Light.h"
class PointLight :
	public Light
{
public:
	Data3D * pos;
	PointLight();
	PointLight(Data3D * dir, MyColor * color) {
		this->color = color;
		this->pos = dir;
	}

	~PointLight();

	// Inherited via Light
	virtual bool isEffective(Data3D & point, Container & bvh) override;
	virtual Data3D getPointToLight(const Data3D & point) override;
	virtual float getAttValue(Data3D & point, Attenuation & attenuation) override;
};


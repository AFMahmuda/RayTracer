#pragma once
#include "Light.h"
class DirectionalLight :
	public Light
{
public:
	Data3D * dir;
	DirectionalLight();
	DirectionalLight(Data3D * dir, MyColor * color) {
		this->color = color;
		this->dir = dir;
	}



	~DirectionalLight();

	// Inherited via Light
	virtual bool isEffective(Data3D & point, Container & bvh) override;
	virtual Data3D getPointToLight(const Data3D & point) override;
	virtual float getAttValue(Data3D& point, Attenuation& att) override { return 1; }

};


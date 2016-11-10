#pragma once
#include "Light.h"
class PointLight :
	public Light
{
public:
	Vec3 * pos;
	PointLight();
	PointLight(Vec3 * dir, MyColor * color) {
		this->color = color;
		this->pos = dir;
	}

	~PointLight();

	// Inherited via Light
	virtual bool isEffective(Vec3 & point, std::shared_ptr<Container> bvh) override;
	virtual Vec3 getPointToLight(const Vec3 & point) override; 
	virtual float getAttValue(Vec3 & point, Attenuation & attenuation) override;
};
 

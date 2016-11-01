#pragma once
#include "Light.h"
class PointLight :
	public Light
{
public:
	Point3 * pos;
	PointLight();
	PointLight(Point3 * dir, MyColor * color) {
		this->color = color;
		this->pos = dir;
	}

	~PointLight();

	// Inherited via Light
	virtual bool isEffective(Point3 & point, Container & bvh) override;
	virtual Vec3 getPointToLight(const Point3 & point) override;
	virtual float getAttValue(Point3 & point, Attenuation & attenuation) override;
};


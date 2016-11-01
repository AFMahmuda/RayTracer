#pragma once
#include "Light.h"
class DirectionalLight :
	public Light
{
public:
	Vec3 * dir;
	DirectionalLight();
	DirectionalLight(Vec3 * dir, MyColor * color) {
		this->color = color;
		this->dir = dir;
	}



	~DirectionalLight();

	// Inherited via Light
	virtual bool isEffective(Point3 & point, Container & bvh) override;
	virtual Vec3 getPointToLight(const Point3 & point) override;
	virtual float getAttValue(Point3& point, Attenuation& att) override { return 1; }

};


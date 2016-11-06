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
	virtual bool isEffective(Vec3 & point, std::shared_ptr< Container> bvh) override;
	virtual Vec3 getPointToLight(const Vec3 & point) override;
	virtual float getAttValue(Vec3& point, Attenuation& att) override { return 1; }

};


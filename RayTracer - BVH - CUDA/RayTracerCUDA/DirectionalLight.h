#pragma once
#include "Light.h"
class DirectionalLight :
	public Light
{
public:
	vec3 * dir;
	DirectionalLight();
	DirectionalLight(vec3 * dir, MyColor * color) {
		this->color = color;
		this->dir = dir;
	}



	~DirectionalLight();

	// Inherited via Light
	virtual bool isEffective(vec3 & point, std::shared_ptr< Container> bvh) override;
	virtual vec3 getPointToLight(const vec3 & point) override;
	virtual float getAttValue(vec3& point, Attenuation& att) override { return 1; }

};


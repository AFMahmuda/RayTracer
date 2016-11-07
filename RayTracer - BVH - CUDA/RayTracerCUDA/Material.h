#pragma once
#include "MyColor.h"
class Material
{
public:
	MyColor specular;
	MyColor diffuse;
	MyColor emmission;
	MyColor ambient;
	float shininess;
	float refractIndex;
	float refractValue;
	Material& operator=(const Material& material) {
		diffuse = material.diffuse;
		specular = material.specular;
		emmission = material.emmission;
		ambient = material.ambient;
		shininess = material.shininess;
		refractIndex = material.refractIndex;
		refractValue = material.refractValue;
	}

	void setShininess(float val);
	void setrefIndex(float val);
	void setRefValue(float val);

	~Material();
	Material();
};


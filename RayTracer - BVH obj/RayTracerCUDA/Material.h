#pragma once
#include "MyColor.h"
#include <string>
class Material
{
public:
	MyColor specular;
	MyColor diffuse;
	MyColor emmission;
	MyColor ambient;
	float shininess;
	float refractIndex;
	float refrVal;
	MyColor reflVal;
	std::string name;
	Material& operator=(const Material& material) {
		diffuse = material.diffuse;
		specular = material.specular;
		emmission = material.emmission;
		ambient = material.ambient;
		shininess = material.shininess;
		refractIndex = material.refractIndex;
		refrVal = material.refrVal;
		name = material.name;
		reflVal = material.reflVal;
		return *this;
	}

	void setShininess(float val);
	void setrefIndex(float val);
	void setRefValue(float val);

	~Material();
	Material();
};


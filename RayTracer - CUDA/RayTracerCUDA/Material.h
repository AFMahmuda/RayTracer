#pragma once
#include "MyColor.h"
class Material
{
public:
	MyColor * Specular;
	MyColor * Diffuse;
	MyColor * Emmission;
	float shininess;
	float refractIndex;
	float refractValue;

	Material();

	void setShininess(float val);
	void setrefIndex(float val);
	void setRefValue(float val);

	~Material();
};


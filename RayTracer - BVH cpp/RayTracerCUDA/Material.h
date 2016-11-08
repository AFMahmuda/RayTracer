#pragma once
#include "MyColor.h"
#include<memory>
class Material
{
public:
	MyColor specular;
	MyColor diffuse;
	MyColor emmission;
	float shininess;
	float refractIndex;
	float refractValue;

	void setShininess(float val);
	void setrefIndex(float val);
	void setRefValue(float val);

	~Material();
	Material();
};


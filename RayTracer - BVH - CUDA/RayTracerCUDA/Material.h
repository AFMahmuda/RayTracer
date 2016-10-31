#pragma once
#include "MyColor.h"
#include<memory>
using namespace std;
class Material
{
public:
	shared_ptr<MyColor> Specular;
	shared_ptr<MyColor> Diffuse;
	shared_ptr<MyColor> Emmission;
	float shininess;
	float refractIndex;
	float refractValue;

	Material();

	void setShininess(float val);
	void setrefIndex(float val);
	void setRefValue(float val);

	~Material();
};


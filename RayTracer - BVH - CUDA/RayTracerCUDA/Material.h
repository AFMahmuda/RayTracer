#pragma once
#include "MyColor.h"
#include<memory>
using namespace std;
class Material
{
public:
	MyColor Specular;
	MyColor Diffuse;
	MyColor Emmission;
	float shininess;
	float refractIndex;
	float refractValue;


	void setShininess(float val);
	void setrefIndex(float val);
	void setRefValue(float val);

	~Material();
	Material();
};


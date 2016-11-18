#include "Material.h"



Material::Material()
{
	specular = MyColor();
	diffuse = MyColor();
	emmission = MyColor();
	ambient = MyColor();

}

void Material::setShininess(float val)
{
	shininess = val;
	if (shininess < 0)
		shininess = 0;
	if (shininess > 255)
		shininess = 255;
}

void Material::setrefIndex(float val)
{
	refractIndex = val;
}

void Material::setRefValue(float val)
{
	refrVal = val;
	if (refrVal < 0)
		refrVal = 0;
	if (refrVal > 1)
		refrVal = 1;
}


Material::~Material()
{
}

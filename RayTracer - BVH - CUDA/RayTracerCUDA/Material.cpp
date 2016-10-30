#include "Material.h"



Material::Material()
{
}

 void Material::setShininess(float val)
{
	shininess = val;
	if (shininess < 0)
		shininess = 0;
	if (shininess > 128)
		shininess = 128;
}

 void Material::setrefIndex(float val)
{
	refractIndex = val;
	if (refractIndex < 0)
		refractIndex = 0;
	if (refractIndex > 1)
		refractIndex = 1;
}

 void Material::setRefValue(float val)
{
	refractValue = val;
	if (refractValue < 0)
		refractValue = 0;
	if (refractValue > 1)
		refractValue = 1;
}


Material::~Material()
{
}

#include "SphereContainer.h"





SphereContainer::~SphereContainer()
{
}

bool SphereContainer::IsIntersecting(Ray& ray)
{
	Vec3 rayToSphere = Vec3(ray.start, this->c);

	float a = ray.direction * ray.direction;
	float b = -2 * (rayToSphere * ray.direction);
	float c = (rayToSphere * rayToSphere) - (r * r);
	float dd = (b * b) - (4 * a * c);

	return (dd > 0);
}

void SphereContainer::showInfo()
{
	std::cout << "c : " << c[0] << "\t" << c[1] << "\t" << c[2] << "\tr: " << r << std::endl;
}

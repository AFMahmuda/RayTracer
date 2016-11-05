#include "Sphere.h"

Sphere::Sphere(Data3D & center, float radius) {
	c = center;
	r = radius;
	trans.matrix = Matrix(4, 4).Identity();
	hasMorton = false;
	updatePos();
	type = SPHERE;
}

bool Sphere::isIntersecting(Ray & ray)
{
	Data3D rayToSphere = Data3D(ray.start, this->c);

	//sphere is behind ray
	//if (rayToSphere * ray.Direction <= 0)
	//    return false;

	float a = ray.direction * ray.direction;
	float b = -2 * (rayToSphere * ray.direction);
	float c = (rayToSphere * rayToSphere) - (this->r * this->r);
	float dd = (b * b) - (4 * a * c);

	if (dd > 0)
	{
		float res1 = (-b + sqrtf(dd)) / (2.0f * a);
		float res2 = (-b - sqrtf(dd)) / (2.0f * a);
		float distance;

		// if both results are negative, then the sphere is behind our ray, 
		// but we already checked that.
		if (res1 < 0 && res2 < 0)
			return false;

		distance = (res1 * res2 < 0) ? std::max(res1, res2) : std::min(res1, res2);

		if (ray.isCloser(distance, trans))
		{
			ray.intersectDist = Matrix::Mul44x41(trans.matrix, ray.direction * distance).Magnitude();
			return true;
		}
	}
	return false;
}

Data3D Sphere::getNormal(Data3D & point)
{
	Data3D p = Matrix::Mul44x41(Matrix(trans.matrix.Inverse()), point);
	Data3D res = Data3D(c, p);
	res = res.Normalize();
	return res;
}

Sphere::~Sphere()
{
}

void Sphere::updatePos()
{

	pos = Data3D();
	pos = Matrix::Mul44x41(trans.matrix, c);

	Data3D p = pos;
	p[0] = (p[0] / 100.f) + .5f;
	p[1] = (p[1] / 100.f) + .5f;
	p[2] = (p[2] / 100.f) + .5f;

	pos = p;
	getMortonPos();

}

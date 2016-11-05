#include "Sphere.h"

Sphere::Sphere(vec3 & center, float radius) {
	c = center;
	r = radius;
	trans.matrix = Matrix(4, 4).Identity();
	hasMorton = false;
	updatePos();
	type = SPHERE;
}

bool Sphere::isIntersecting(Ray & ray)
{
	vec3 rayToSphere = vec3(ray.start, this->c);

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

vec3 Sphere::getNormal(vec3 & point)
{
	vec3 p = Matrix::Mul44x41(Matrix(trans.matrix.Inverse()), point);
	vec3 res = vec3(c, p);
	res = res.Normalize();
	return res;
}

Sphere::~Sphere()
{
}

void Sphere::updatePos()
{

	pos = vec3();
	pos = Matrix::Mul44x41(trans.matrix, c);

	vec3 p = pos;
	p[0] = (p[0] / 100.f) + .5f;
	p[1] = (p[1] / 100.f) + .5f;
	p[2] = (p[2] / 100.f) + .5f;

	pos = p;
	getMortonPos();

}

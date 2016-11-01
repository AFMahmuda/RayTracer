#pragma once
#include"Geometry.h"
#include<iostream>

class Sphere : public Geometry
{


public:
	Point3 c; //center
	float r; //radius

	Sphere(float * val) :Sphere(Point3(val[0], val[1], val[2]), val[3]) {}
	Sphere() {}
	Sphere(Point3& center, float radius) {
		c = center;
		r = radius;
		trans.matrix = Matrix(4, 4).Identity();
		hasMorton = false;
		updatePos();
		type = SPHERE;
	}

	bool isIntersecting(Ray& ray)
	{
		Vec3 rayToSphere = Vec3(ray.start, this->c);

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
				ray.intersectDist = Vec3(Matrix::Mul44x41(trans.matrix, ray.direction * distance)).Magnitude();
				return true;
			}
		}
		return false;
	}

	Vec3 getNormal(Point3& point)
	{
		Point3 p = Matrix::Mul44x41(Matrix(trans.matrix.Inverse()), point);
		Vec3 * res = new Vec3(c, p);
		res->Normalize();
		return *res;
	}


	~Sphere();

	// Inherited via Geometry
	virtual void updatePos() override;
};


#pragma once

#include<memory>
#include<vector>

//#include"Scene.h"
//#include"Container.h"
//#include"Light.h"
//#include"MyColor.h"
//#include"Matrix.h"

#include"Transform.h"

#include"Point3.h"
#include"Vec3.h"

class Geometry;//forward declaration
class Container;//forward declaration
//class Light;//forward declaration
class Ray
{
public:
	enum  TYPE {
		RAY, REFLECTION, REFRACTION
	};

	TYPE type;
	Point3 start;
	Vec3 direction;
	float intersectDist = INT_MAX;
	std::shared_ptr< Geometry > intersectWith;
	Point3& getHitReal() { return Point3(start + Point3(direction * intersectDist)); }
	Point3& getHitPlus() { return Point3(start + Point3(direction * (intersectDist * 1.001f))); }
	Point3& getHitMin() { return Point3(start + Point3(direction * (intersectDist * 0.999f))); }


	//MyColor& getColor(Scene& scene, int bounce) {
	//	if (bounce <= 0 || intersectWith == nullptr)
	//	{
	//		if (type == REFRACTION || type == REFRACTION)
	//			return MyColor();
	//		return MyColor();
	//		//return scene.defColor;
	//	}

	//	else
	//	{
	//		vector<shared_ptr< Light>> effectiveLights = populateLights(scene.lights, *scene.container);
	//		MyColor color = calcColor(effectiveLights, scene.att);

	//		return color +
	//			getRefl(scene, bounce - 1) +
	//			getRefr(scene, bounce - 1);
	//	}
	//}
	//MyColor& getRefl(Scene& scene, int bounce) {
	//	//TODO : IMPLEMENT
	//	return MyColor();
	//}
	//MyColor& getRefr(Scene& scene, int bounce) {
	//	//TODO : IMPLEMENT
	//	return MyColor();
	//}
	//MyColor& calcColor(vector<shared_ptr< Light>> effectiveLights, Attenuation& attenuation)
	//{
	//	//TODO : IMPLEMENT
	//	return MyColor();
	//}

	//vector<shared_ptr< Light> >populateLights(vector < shared_ptr< Light>> allLights, Container& bvh)
	//{
	//	vector <shared_ptr<Light>> res;
	//	for (size_t i = 0; i < allLights.size(); i++)
	//	{
	//		if (allLights[i]->isEffective(getHitMin(), bvh))
	//			res.push_back(allLights[i]);
	//	}
	//}

	void trans(Transform& transform) {
		start = Point3(Matrix::Mul44x41(transform.matrix, start));
		direction = Vec3(Matrix::Mul44x41(transform.matrix, direction)).Normalize();
	}
	void transInv(Transform& transform) {
		start = Point3(Matrix::Mul44x41((Matrix)transform.matrix.Inverse(), start));
		direction = Vec3(Matrix::Mul44x41((Matrix)transform.matrix.Inverse(), direction)).Normalize();

	}
	bool isCloser(float dist, Transform& trans)
	{
		float newMag = Vec3(Matrix::Mul44x41(trans.matrix, direction * dist)).Magnitude();
		return (newMag < intersectDist) ? true : false;
	}

	Ray();
	~Ray();
};



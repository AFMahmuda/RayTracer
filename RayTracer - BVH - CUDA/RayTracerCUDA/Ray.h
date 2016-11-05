#pragma once

#include<memory>
#include<vector>

//#include"Scene.h"
//#include"Container.h"
//#include"Light.h"
//#include"MyColor.h"
//#include"Matrix.h"

#include"Transform.h"

#include"Data3D.h"

class Geometry;//forward declaration
class Ray
{
public:
	enum  TYPE {
		RAY, REFLECTION, REFRACTION
	};

	TYPE type;
	Data3D start;
	Data3D direction;
	float intersectDist = FLT_MAX;
	std::shared_ptr< Geometry > intersectWith;
	Data3D getHitReal() const { return Data3D(start + Data3D(direction * intersectDist)); }
	Data3D getHitPlus() const { return Data3D(start + Data3D(direction * (intersectDist * 1.01f))); }
	Data3D getHitMin() const { return Data3D(start + Data3D(direction * (intersectDist * 0.99f))); }

	void trans(Transform& transform);
	void transInv(Transform& transform);
	bool isCloser(float dist, Transform& trans);

	Ray();
	Ray(Data3D start, Data3D dir) :start(start), direction(dir) {}
	~Ray();
};



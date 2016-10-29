#pragma once
#include"Point3.h"
#include"Vec3.h"
//#define _USE_MATH_DEFINES
#include <cmath>
#ifndef M_PI
#define M_PI 3.14159265358979323846
#endif
class Camera
{
private:


public:

	static Camera& Instance();

	Vec3 U;
	Vec3 V;
	Vec3 W;

	float fov;

	Vec3 up;
	Point3 pos;
	Point3 lookAt;

	void Init(float * vals);
	void Init(Point3& pos, Point3& lookAt, Vec3& up, float fov);

	Vec3& CameraViewPosition();

protected:

	Camera(); // Prevent construction
	Camera(const Camera&); // Prevent construction by copying
	Camera& operator=(const Camera&); // Prevent assignment
	~Camera(); // Prevent unwanted destruction
};


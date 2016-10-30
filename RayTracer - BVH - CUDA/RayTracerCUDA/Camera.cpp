#include "Camera.h"

bool Camera::flag = false;
Camera * Camera::instance = nullptr;

void Camera::Init(float * vals) {
	Init(
		Point3(vals[0], vals[1], vals[2]),
		Point3(vals[3], vals[4], vals[5]),
		Vec3(vals[6], vals[7], vals[8]),
		vals[9]
		);
}

void Camera::Init(Point3& pos, Point3& lookAt, Vec3& up, float fov) {
	this->pos = pos;
	this->lookAt = lookAt;
	this->up = up;
	this->fov = fov;

	W = Vec3(pos, lookAt).Normalize();
	U = Vec3::Cross(this->up, W).Normalize();
	V = Vec3::Cross(W, U);
}

Vec3& Camera::CameraViewPosition()
{
	float	x = pos[0];
	float	y = pos[1];
	float	z = pos[2];
	return Vec3((U * x) + (V * y) + (W * z));
}

Camera::Camera()
{
	Init(Point3(), Point3(), Vec3(), 15);
}

Camera::Camera(const Camera &)
{
}

Camera::~Camera()
{
}

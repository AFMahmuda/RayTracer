#include "Camera.h"

bool Camera::flag = false;
Camera * Camera::instance = nullptr;

void Camera::Init(float * vals) {
	Init(
		Vec3(vals[0], vals[1], vals[2], 1),//pos
		Vec3(vals[3], vals[4], vals[5], 1),//lookat
		Vec3(vals[6], vals[7], vals[8], 0),//up
		vals[9]//fov
		);
}

void Camera::Init(Vec3& pos, Vec3& lookAt, Vec3& up, float fov) {
	this->pos = pos;
	this->lookAt = lookAt;
	this->up = up;
	this->fov = fov;

	W = Vec3(pos, lookAt).normalize();
	U = Vec3::Cross(this->up, W).normalize();
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
	//	Init(Vec3(0, 0, 0, 1), Vec3(0, 0, 0, 1), Vec3(0, 0, 0, 0), 15);
}

Camera::Camera(const Camera &)
{
}

Camera::~Camera()
{
}

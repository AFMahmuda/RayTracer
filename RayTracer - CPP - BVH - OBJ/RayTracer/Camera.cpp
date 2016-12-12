#include "Camera.h"

bool Camera::flag = false;
Camera * Camera::instance = nullptr;

void Camera::init(float * vals) {
	init(
		Vec3(vals[0], vals[1], vals[2], 1),//pos
		Vec3(vals[3], vals[4], vals[5], 1),//lookat
		Vec3(vals[6], vals[7], vals[8], 0),//up
		vals[9]//fov
		);
}

void Camera::init(Vec3& pos, Vec3& lookAt, Vec3& up, float fov) {
	this->pos = pos;
	this->lookAt = lookAt;
	this->up = up;
	this->fov = fov;

	w = Vec3(pos, lookAt).normalize();
	u = Vec3::Cross(this->up, w).normalize();
	v = Vec3::Cross(w, u);
}

Vec3& Camera::cameraViewPosition()
{
	float	x = pos[0];
	float	y = pos[1];
	float	z = pos[2];
	return Vec3((u * x) + (v * y) + (w * z));
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

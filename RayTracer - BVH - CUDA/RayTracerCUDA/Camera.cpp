#include "Camera.h"

bool Camera::flag = false;
Camera * Camera::instance = nullptr;

void Camera::Init(float * vals) {
	Init(
		vec3(vals[0], vals[1], vals[2], 1),
		vec3(vals[3], vals[4], vals[5], 1),
		vec3(vals[6], vals[7], vals[8], 0),
		vals[9]
		);
}

void Camera::Init(vec3& pos, vec3& lookAt, vec3& up, float fov) {
	this->pos = pos;
	this->lookAt = lookAt;
	this->up = up;
	this->fov = fov;

	W = vec3(pos, lookAt).Normalize();
	U = vec3::Cross(this->up, W).Normalize();
	V = vec3::Cross(W, U);
}

vec3& Camera::CameraViewPosition()
{
	float	x = pos[0];
	float	y = pos[1];
	float	z = pos[2];
	return vec3((U * x) + (V * y) + (W * z));
}

Camera::Camera()
{
	Init(vec3(0, 0, 0, 1), vec3(0, 0, 0, 1), vec3(0, 0, 0, 0), 15);
}

Camera::Camera(const Camera &)
{
}

Camera::~Camera()
{
}

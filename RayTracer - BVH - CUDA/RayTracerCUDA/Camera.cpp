#include "Camera.h"

bool Camera::flag = false;
Camera * Camera::instance = nullptr;

void Camera::Init(float * vals) {
	Init(
		Data3D(vals[0], vals[1], vals[2], 1),
		Data3D(vals[3], vals[4], vals[5], 1),
		Data3D(vals[6], vals[7], vals[8], 0),
		vals[9]
		);
}

void Camera::Init(Data3D& pos, Data3D& lookAt, Data3D& up, float fov) {
	this->pos = pos;
	this->lookAt = lookAt;
	this->up = up;
	this->fov = fov;

	W = Data3D(pos, lookAt).Normalize();
	U = Data3D::Cross(this->up, W);
	U = U.Normalize();
	V = Data3D::Cross(W, U);
}

Data3D& Camera::CameraViewPosition()
{
	float	x = pos[0];
	float	y = pos[1];
	float	z = pos[2];
	return Data3D((U * x) + (V * y) + (W * z));
}

Camera::Camera()
{
	Init(Data3D(0, 0, 0, 1), Data3D(0, 0, 0, 1), Data3D(0, 0, 0, 0), 15);
}

Camera::Camera(const Camera &)
{
}

Camera::~Camera()
{
}

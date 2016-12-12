#include "ViewPlane.h"
#define _USE_MATH_DEFINES
#include <cmath>
//#ifndef M_PI
//#define M_PI 3.14159265358979323846
//#endif

bool ViewPlane::flag = false;
ViewPlane * ViewPlane::instance = nullptr;

void ViewPlane::init(int width, int height)
{
	pixelH = height;
	pixelW = width;
	worldH = 2.f * tanf((Camera::getInstance()->fov / 2.f) * (float)M_PI / 180.f);
	worldW = worldH * (pixelW / (float)pixelH);

	precalculate();

}

void ViewPlane::precalculate()
{
	setUpperleft();
	unitRight = ((Camera::getInstance()->u * (worldW / (float)pixelW)) * -1.f);
	unitDown = ((Camera::getInstance()->v * (worldH / (float)pixelH)) * -1.f);
}

void ViewPlane::setUpperleft()
{
	Vec3 c = Camera::getInstance()->pos;
	c = c + (Camera::getInstance()->w);
	pos = c;
	upperLeft =
		c
		+ (Camera::getInstance()->u * (worldW * .5f))
		+ (Camera::getInstance()->v * (worldH * .5f));
	upperLeft[3] = 1;
}

Vec3 ViewPlane::getNewLocation(int col, int row)
{
	Vec3 uleft = upperLeft;
	Vec3 uRight = unitRight;
	Vec3 uDown = unitDown;
	Vec3 res =
		uleft +
		uRight * (col + .5f)
		+ uDown * (row + .5f)
		;
	res[3] = 1;
	return res;

}

ViewPlane::ViewPlane()
{
//	Init(1, 1);
}

ViewPlane::ViewPlane(const ViewPlane &)
{
}
//
//ViewPlane & ViewPlane::operator=(const ViewPlane &)
//{
//	return Instance();
//}


ViewPlane::~ViewPlane()
{
}

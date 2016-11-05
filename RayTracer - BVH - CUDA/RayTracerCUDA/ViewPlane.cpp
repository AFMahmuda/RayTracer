#include "ViewPlane.h"
//#define _USE_MATH_DEFINES
//#include <cmath>  
#ifndef M_PI
#define M_PI 3.14159265358979323846
#endif

bool ViewPlane::flag = false;
ViewPlane * ViewPlane::instance = nullptr;

void ViewPlane::Init(int width, int height)
{
	pixelH = height;
	pixelW = width;
	worldH = 2.f * tanf((Camera::Instance()->fov / 2.f) * (float)M_PI / 180.f);
	worldW = worldH * (pixelW / (float)pixelH);

	Precalculate();

}

void ViewPlane::Precalculate()
{
	upperLeft = getUpperLeft();
	unitRight = ((Camera::Instance()->U * (worldW / (float)pixelW)) * -1.f);
	unitDown = ((Camera::Instance()->V * (worldH / (float)pixelH)) * -1.f);
}

vec3 & ViewPlane::getUpperLeft()
{
	vec3 c = Camera::Instance()->pos;
	c = c + (Camera::Instance()->W);
	pos = c;
	vec3 res =
		c
		+ (Camera::Instance()->U * (worldW * .5f))
		+ (Camera::Instance()->V * (worldH * .5f));
	res[3] = 1;
	return res;
}

vec3 ViewPlane::getNewLocation(int col, int row)
{
	vec3 uleft = upperLeft;
	vec3 uRight = unitRight;
	vec3 uDown = unitDown;
	vec3 res =
		uleft +
		uRight * (col + .5f)
		+ uDown * (row + .5f)
		;
	res[3] = 1;
	return res;

}

ViewPlane::ViewPlane()
{
	Init(1, 1);
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

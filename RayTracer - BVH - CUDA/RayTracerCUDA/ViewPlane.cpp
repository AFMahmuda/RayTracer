#include "ViewPlane.h"

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

Point3 & ViewPlane::getUpperLeft()
{
	Point3 c = Camera::Instance()->pos;
	c = c + Point3(Camera::Instance()->W);
	pos = c;
	Point3 res =
		c
		+ Point3(Camera::Instance()->U * (worldW * .5f))
		+ Point3(Camera::Instance()->V * (worldH * .5f));

	return res;
}

Point3& ViewPlane::getNewLocation(int col, int row)
{
	Point3 uleft = upperLeft;
	Vec3 uRight = unitRight;
	Vec3 uDown = unitDown;

	return	uleft + Point3(
		uRight * (col + .5f)
		+ uDown * (row + .5f))
		;

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

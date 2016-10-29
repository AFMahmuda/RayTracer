#include "ViewPlane.h"



ViewPlane & ViewPlane::Instance()
{
	static ViewPlane inst;
	return inst;
}

void ViewPlane::Init(int width, int height)
{
	pixelH = height;
	pixelW = width;
	worldH = 2.f * tanf((Camera::Instance().fov / 2.f) * (float)M_PI / 180.f);
	worldW = worldH * (pixelW / (float)pixelH);

	Precalculate();

}

void ViewPlane::Precalculate()
{
	upperLeft = getUpperLeft();
	unitRight = ((Camera::Instance().U * (worldW / (float)pixelW)) * -1.f);
	unitRight = ((Camera::Instance().V * (worldH / (float)pixelH)) * -1.f);
}

Point3 & ViewPlane::getUpperLeft()
{
	Point3 c = Camera::Instance().pos;
	c = c + Camera::Instance().W;
	pos = c;
	Point3 upperLeft =
		c
		+ Camera::Instance().U * (worldW * .5f)
		+ Camera::Instance().V * (worldH * .5f);

		return upperLeft;
}

Point3& ViewPlane::getNewLocation(int col, int row)
{
	Point3 uleft = upperLeft;
	Vec3 uRight = unitRight;
	Vec3 uDown = unitDown;

	return		uleft
		+ uRight * (col + .5f)
		+ uDown * (row + .5f)
		;

}

ViewPlane::ViewPlane()
{
}

ViewPlane::ViewPlane(const ViewPlane &)
{
}

ViewPlane & ViewPlane::operator=(const ViewPlane &)
{
	return Instance();
}


ViewPlane::~ViewPlane()
{
}

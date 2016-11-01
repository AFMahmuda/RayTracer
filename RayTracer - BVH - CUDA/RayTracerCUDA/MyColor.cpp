#include "MyColor.h"



 MyColor::MyColor(float r, float g, float b):r(r),g(g),b(b) {}

 void MyColor::setR(float val)
{
	r = (val < 0) ? 0 : ((val>1) ? 1 : val);
}

 void MyColor::setG(float val)
{
	g = (val < 0) ? 0 : ((val>1) ? 1 : val);
}

 void MyColor::setB(float val)
{
	b = (val < 0) ? 0 : ((val>1) ? 1 : val);
}

MyColor::~MyColor()
{
}

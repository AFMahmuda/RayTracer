#include "Geometry.h"

void Geometry::setTrans(const Transform & trans)
{
	Geometry::trans.matrix = trans.matrix;	
	hasMorton = false;
	updatePos();
	return;
}

unsigned int Geometry::getMortonPos()
{
	if (!hasMorton)
	{
		unsigned int x = expandBits(fminf(fmaxf(pos[0] * 1024.f, 0.f), 1023.f));
		unsigned int y = expandBits(fminf(fmaxf(pos[1] * 1024.f, 0.f), 1023.f));
		unsigned int z = expandBits(fminf(fmaxf(pos[2] * 1024.f, 0.f), 1023.f));
		mortonCode = x * 4 + y * 2 + z;
		hasMorton = true;
	}
	return mortonCode;
}

unsigned int Geometry::expandBits(unsigned int v)
{
	v = (v * 0x00010001u) & 0xFF0000FFu;
	v = (v * 0x00000101u) & 0x0F00F00Fu;
	v = (v * 0x00000011u) & 0xC30C30C3u;
	v = (v * 0x00000005u) & 0x49249249u;
	return v;
}

std::string Geometry::getMortonBitString()
{
	return std::bitset<30>(getMortonPos()).to_string();
}

Geometry::Geometry():
mat(Material()),ambient(MyColor(0,0,0))
{
}


Geometry::~Geometry()
{
}

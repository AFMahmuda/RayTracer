#pragma once

class Ray
{
public:
	enum  TYPE
	{
		RAY, REFLECTION, REFRACTION
	};
	TYPE type;


	Ray();


	~Ray();
};


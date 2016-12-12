#pragma once
#include "Vec3.h"
class Camera
{
private:
	static bool flag;
	static Camera* instance;

public:

	static Camera* getInstance()
	{
		if (flag == false)
		{
			instance = new Camera();
			flag = true;
		}		
		return instance;
	}

	Vec3 u;
	Vec3 v;
	Vec3 w;

	float fov;

	Vec3 up;
	Vec3 pos;
	Vec3 lookAt;

	void init(float * vals);
	void init(Vec3& pos, Vec3& lookAt, Vec3& up, float fov);

	Vec3& cameraViewPosition();

protected:

	Camera(); // Prevent construction
	Camera(const Camera&); // Prevent construction by copying
	~Camera(); // Prevent unwanted destruction
};


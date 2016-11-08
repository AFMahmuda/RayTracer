#pragma once
#include "Vec3.h"
class Camera
{
private:
	static bool flag;
	static Camera* instance;

public:

	static Camera* Instance()
	{
		if (flag == false)
		{
			instance = new Camera();
			flag = true;
		}		
		return instance;
	}

	Vec3 U;
	Vec3 V;
	Vec3 W;

	float fov;

	Vec3 up;
	Vec3 pos;
	Vec3 lookAt;

	void Init(float * vals);
	void Init(Vec3& pos, Vec3& lookAt, Vec3& up, float fov);

	Vec3& CameraViewPosition();

protected:

	Camera(); // Prevent construction
	Camera(const Camera&); // Prevent construction by copying
//	Camera& operator=(const Camera&); // Prevent assignment
	~Camera(); // Prevent unwanted destruction
};


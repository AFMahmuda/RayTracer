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

	vec3 U;
	vec3 V;
	vec3 W;

	float fov;

	vec3 up;
	vec3 pos;
	vec3 lookAt;

	void Init(float * vals);
	void Init(vec3& pos, vec3& lookAt, vec3& up, float fov);

	vec3& CameraViewPosition();

protected:

	Camera(); // Prevent construction
	Camera(const Camera&); // Prevent construction by copying
//	Camera& operator=(const Camera&); // Prevent assignment
	~Camera(); // Prevent unwanted destruction
};


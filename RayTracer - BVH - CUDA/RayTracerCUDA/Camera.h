#pragma once
#include"Data3D.h"

//#ifndef M_PI
//#define M_PI 3.14159265358979323846
//#endif
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

	Data3D U;
	Data3D V;
	Data3D W;

	float fov;

	Data3D up;
	Data3D pos;
	Data3D lookAt;

	void Init(float * vals);
	void Init(Data3D& pos, Data3D& lookAt, Data3D& up, float fov);

	Data3D& CameraViewPosition();

protected:

	Camera(); // Prevent construction
	Camera(const Camera&); // Prevent construction by copying
//	Camera& operator=(const Camera&); // Prevent assignment
	~Camera(); // Prevent unwanted destruction
};


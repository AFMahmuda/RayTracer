#pragma once
#include <cuda_runtime.h>
#include "cublas_v2.h"

class MyCUDAHandler
{
private:
	static bool flag;
	static MyCUDAHandler* instance;
public:
	static MyCUDAHandler* getInstance()
	{
		if (flag == false)
		{
			instance = new MyCUDAHandler();
			flag = true;
		}
		return instance;
	}

protected:
	MyCUDAHandler();
	~MyCUDAHandler();
};


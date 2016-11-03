#include "Data3D.h"
#include <cuda_runtime.h>
#include "cublas_v2.h"
#include <iostream>

Data3D& Data3D::operator+=(const Data3D& rhs) {

	cudaError_t cudaStat; // cudaMalloc status
	cublasStatus_t stat; // CUBLAS functions status
	cublasHandle_t handle; // CUBLAS context
	float *x, *y; // n- vector on the host
	x = v;
	y = rhs.v;
	float * d_x; // d_x - x on the device
	float * d_y; // d_y - y on the device
	cudaMalloc((void **)& d_x, n * sizeof(*x)); // device
														   // memory alloc for x
	cudaMalloc((void **)& d_y, n * sizeof(*y)); // device
														   // memory alloc for y
	stat = cublasCreate(&handle); // initialize CUBLAS context
	stat = cublasSetVector(n, sizeof(*v), x, 1, d_x, 1); // cp x- >d_x
	stat = cublasSetVector(n, sizeof(*y), y, 1, d_y, 1); // cp y- >d_y
	float s = 1;
	stat = cublasSaxpy(handle, n, &s, d_x, 1, d_y, 1); // d_y = s*d_x+d_y
	stat = cublasGetVector(n, sizeof(float), d_y, 1, y, 1); // cp d_y - >y
	v = y; //this-> v  = y
	cudaFree(d_x); // free device memory
	cudaFree(d_y); // free device memory
	cublasDestroy(handle); // destroy CUBLAS context

	return *this;
}


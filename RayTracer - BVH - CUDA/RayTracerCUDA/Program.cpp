#include"TraceManager.h"

#include<string>
#include<iostream>
#include<fstream>
using namespace std;
int main(int argc, char *argv[])
{
	string filename = "E:/5112100118 - Fathur/TA/Project/RayTracer/RayTracer - BVH - CUDA/x64/Debug/default.test";

	if (argc >= 2)
		filename = argv[1];

	ifstream myfile(filename);

	if (myfile.is_open())
	{
		myfile.close();
		TraceManager(1, Container::BOX, false).traceScene(filename);
	}
	else
	{
		cout << filename << " file not found!" << endl;
	}

	int a;
	cin >> a;

	return 0;
}
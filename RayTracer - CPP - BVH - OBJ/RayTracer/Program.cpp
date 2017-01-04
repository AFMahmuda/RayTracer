#include <fstream>  //file 
#include <iostream>
#include <algorithm> //replace
#include <string>
#include "TraceManager.h"
#include "ThreadPool.h"
#include "Container.h"

using namespace std;

int main(int argc, char *argv[])
{
	//scene file name, default = "default.scene"  
	string filename = (argc >= 2) ? argv[1] : "scene_default.scene";
	std::replace(filename.begin(), filename.end(), '\\', '/');


	bool isAAC = true;

	//bin type (b = box / s = sphere ), default = box;
	Container::TYPE binType = (argc >= 3) ? (argv[2][0] == 'b') ? Container::BOX : Container::SPHERE : Container::BOX;

	//aac treshold, def=20;
	int thres = (argc >= 4) ? (atoi(argv[3])) : 12;


	//thread number for aac and 
	(argc >= 5) ? ThreadPool::setMaxThread((atoi(argv[4]) - 1)) : ThreadPool::setMaxThread(std::thread::hardware_concurrency() - 1);

	//thread number for tracing
	int traceTN = 8 * std::thread::hardware_concurrency();

	ifstream myfile(filename);
	if (myfile.is_open())
	{
		myfile.close();
		TraceManager tracer(traceTN, binType, isAAC, thres);
		tracer.traceScene(filename);
	}
	else
	{
		cout << filename << " file not found!" << endl;
	}
	return 0;
}
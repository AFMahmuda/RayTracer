#include <stdio.h>  // defines FILENAME_MAX
#include <fstream>	//file 
#include <direct.h> // path
#include <iostream>
#include <algorithm>//replace
#include <string>

#include"TraceManager.h"
#include"Container.h"
using namespace std;

int main(int argc, char *argv[])
{
	//scene file name, default = "default.scene"	
	string filename = (argc >= 2) ? argv[1] : "default.scene";
	std::replace(filename.begin(), filename.end(), '\\', '/');
	
	//thread number, default = 1
	int threads = (argc >= 3) ? atoi(argv[2]) : 1;
	
	//using aac algorithm (1 = yes / 0 = no ), default = no;
	bool isAAC = (argc >= 4) ? (atoi(argv[3]) == 1) ? true : false : true;
	
	//bin type (b = box / s = sphere ), default = box;
	Container::TYPE binType = (argc >= 5) ? (argv[4][0] == 'b') ? Container::BOX : Container::SPHERE : Container::BOX;
	
	//aac treshold, def=20;
	int thres = (argc >= 6) ? (atoi(argv[5])) : 20;

	ifstream myfile(filename);
	if (myfile.is_open())
	{
		myfile.close();
		TraceManager tracer(threads, binType, isAAC,thres);
		tracer.traceScene(filename);
	}
	else
	{
		cout << filename << " file not found!" << endl;
	}

	system("pause");
	return 0;
}
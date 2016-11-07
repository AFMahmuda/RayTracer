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
	char dir[FILENAME_MAX];
	_getcwd(dir, sizeof(dir));
	strcat(dir, "\\");
	//scene name, default = default.test
	string filename = (argc >= 2) ? strcat(dir, argv[1]) : strcat(dir, "default.test");
	//thread number, default = 1
	int threads = (argc >= 3) ? atoi(argv[2]) : 1;
	//using aac algorithm (1 = yes / 0 = no ), default = no;
	bool isAAC = (argc >= 4) ? (atoi(argv[3]) == 1) ? true : false : false;
	//bin type (b = box / s = sphere ), default = box;
	Container::TYPE binType = (argc >= 5) ? (argv[4][0] == 'b') ? Container::BOX : Container::SPHERE : Container::BOX;

	std::replace(filename.begin(), filename.end(), '\\', '/');

	ifstream myfile(filename);

	if (myfile.is_open())
	{
		cout << filename << endl;
		myfile.close();
		TraceManager tracer(threads, binType, isAAC);
		tracer.traceScene(filename);
	}
	else
	{
		cout << filename << " file not found!" << endl;
	}

	system("pause");
	return 0;
}
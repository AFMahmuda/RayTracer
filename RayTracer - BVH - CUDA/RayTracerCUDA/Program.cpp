#include"TraceManager.h"

#include<string>
#include<iostream>
#include<fstream>


#include <stdio.h>  /* defines FILENAME_MAX */
#include <direct.h>

using namespace std;

int main(int argc, char *argv[])
{
	char dir[FILENAME_MAX];
	_getcwd(dir, sizeof(dir));

	strcat(dir, "\\");
	if (argc >= 2)
		strcat(dir, argv[1]);
	else
		strcat(dir, "default.test");

	string filename = dir;
	std::replace(filename.begin(), filename.end(), '\\', '/');

	ifstream myfile(filename);

	if (myfile.is_open())
	{
		cout << filename << endl;
		myfile.close();
		//		TraceManager(4, Container::BOX, true).traceScene(filename);
		Data3D v1 = Point3(1, 1, 1);
		Data3D v2 = Point3(2, 2, 2);
		v2 += v1;
		Data3D v3 = Point3(2, 2, 2);
	}
	else
	{
		cout << filename << " file not found!" << endl;
	}

	system("pause");
	return 0;
}
#include<string.h>
#include"Sphere.h"
#include"Translation.h"
#include"Rotation.h"
#include"Scaling.h"

#include"TraceManager.h"
#include"RadixSort.h"
using namespace std;
int main(int argc, char *argv[])
{
	char* fileName = "default.test";

	if (argc >= 2)
		fileName = argv[1];

	FILE *file;
	if (file = fopen(fileName, "r"))
	{
		cout << fileName << endl;
	}
	else
	{
		cout << "file not found!" << endl;
		TraceManager(1,Container::BOX,false).traceScene("");		
	}


	int a;
	cin >> a;

	return 0;
}
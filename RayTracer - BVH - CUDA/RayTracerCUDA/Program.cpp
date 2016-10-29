#include<string.h>
#include"Sphere.h"
#include"Translation.h"
#include"Rotation.h"
#include"Scaling.h"


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

		int count = 200;
		Geometry** balz;
		balz = (Geometry**)malloc(sizeof(Geometry*) * count);
		for (int x = 0; x < count; x++)
		{
			float randx = rand() % 100 - 50;
			float randy = rand() % 100 - 50;
			float randz = rand() % 100 - 50;
			balz[x] = new Sphere(Point3(randx, randy, randz), 1);
		}

		RadixSort::radixsort(balz, count);

		for (int x = 0; x < count; x++)
		{
			cout << "sph pos : " << ((Sphere*)balz[x])->c[0] << "\t" << ((Sphere*)balz[x])->c[1] << "\t" << ((Sphere*)balz[x])->c[2] << "\t" << balz[x]->getMortonPos() << endl;

		}


		//Transform trans = Translation(1, 1, 1);
		//Transform scale = Scaling(2, 2, 2);
		//Transform rotat = Rotation(new float[3]{ 1,1,1 }, 45);

		//Transform multi;
		//multi.matrix = Matrix::Mul44x44(trans.matrix, scale.matrix);

		//cout << "trans:" << endl;
		//for (size_t i = 0; i < 4; i++)
		//{
		//	for (size_t j = 0; j < 4; j++)
		//		cout << trans.matrix(i, j) << " ";
		//	cout << endl;
		//}

		//cout << "scale:" << endl;
		//for (size_t i = 0; i < 4; i++)
		//{
		//	for (size_t j = 0; j < 4; j++)
		//		cout << scale.matrix(i, j) << " ";
		//	cout << endl;
		//}

		//cout << "rotat:" << endl;
		//for (size_t i = 0; i < 4; i++)
		//{
		//	for (size_t j = 0; j < 4; j++)
		//		cout << rotat.matrix(i, j) << " ";
		//	cout << endl;
		//}

		//cout << "multi:" << endl;
		//for (size_t i = 0; i < 4; i++)
		//{
		//	for (size_t j = 0; j < 4; j++)
		//		cout << multi.matrix(i, j) << " ";
		//	cout << endl;
		//}





	}


	int a;
	cin >> a;

	return 0;
}
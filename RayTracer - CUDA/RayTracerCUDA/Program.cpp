#include<string.h>
#include"Sphere.h"
#include"Translation.h"
#include"Rotation.h"
#include"Scaling.h"

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
		float min = -50, max = 50, step = 25;

		for (float x = min; x <= max; x += step)
		{
			for (float y = min; y <= max; y += step)
			{
				for (float z = min; z <= max; z += step)
				{
					Sphere balz(Point3(x, y, z), 1);
					cout << "sph pos : " << balz.c[0] << "\t" << balz.c[1] << "\t" << balz.c[2] << "\t" << balz.r << "\t" << balz.getMortonBitString() << endl;
				}
			}

		}
		cout << "end" << endl;


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
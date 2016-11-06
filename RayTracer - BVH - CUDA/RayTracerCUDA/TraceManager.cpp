#include "Tracemanager.h"
#include <ctime>
#include <iostream>
#include <thread>

void TraceManager::initScene(std::string sceneFile) {
	scene = Scene(sceneFile);

	//precalculate w and h measurements
	//w and h total
	height = ViewPlane::Instance()->pixelH;
	width = ViewPlane::Instance()->pixelW;

	//search two closest factors 6 = 3 and 2 , 5 = 5 and 1
	verDiv = (int)sqrtf(tn) - 1;
	do verDiv++; while (tn % verDiv != 0);
	horDiv = tn / verDiv;

	wPerSeg = width / horDiv; //width per segmen
	hPerSeg = height / verDiv; //height per segmen
	outFileName = scene.outFileName;
}

void TraceManager::buildBVH() {
	BVHBuilder(binType, isAAC).BuildBVH(scene);
}

void TraceManager::trace() {
	std::vector<std::thread> threads;
	int cnt = 0;
	image = FreeImage_Allocate(width, height, 24);
	for (int i = 0; i < verDiv; i++)
		for (int j = 0; j < horDiv; j++)
		{
			int row = i, col = j, n = cnt;
			threads.push_back(std::thread(traceThread, image, std::ref(scene), row * hPerSeg, col * wPerSeg, (row + 1) * hPerSeg, (col + 1) * wPerSeg));
			cnt++;
		}

	for (size_t i = 0; i < threads.size(); i++)
	{
		threads[i].join();
	}
}

void TraceManager::traceThread(FIBITMAP * image, Scene &scene, int rowStart, int colStart, int rowEnd, int colEnd)
{
	RayManager& rayManager = RayManager();
	Ray ray;
	vec3 pixPosition;
	RGBQUAD color;
	for (int currRow = rowStart; currRow < rowEnd; currRow++)
	{
		for (int currCol = colStart; currCol < colEnd; currCol++)
		{
			ray = Ray();
			pixPosition = ViewPlane::Instance()->getNewLocation(currCol, currRow);
			ray.start = Camera::Instance()->pos;
			ray.direction = vec3(ray.start, pixPosition).Normalize();

			rayManager.traceRay(ray, scene.bin);

			MyColor& col = RayManager().getColor(ray, scene, scene.maxDepth);
			color = MyColToRGBQUAD(col);

			FreeImage_SetPixelColor(image, currCol, currRow, &color);
		}
	}
}


void TraceManager::mergeAndSaveImage() {
	FreeImage_FlipVertical(image);
	FreeImage_Save(FIF_BMP, image, outFileName.c_str(), 0);
}

RGBQUAD TraceManager::MyColToRGBQUAD(MyColor & col)
{
	RGBQUAD color;
	color.rgbRed = col.r * 255;
	color.rgbGreen = col.g * 255;
	color.rgbBlue = col.b * 255;
	return color;
}

TraceManager::TraceManager(int threadNumber, Container::TYPE _type, bool _isAAC)
{
	tn = threadNumber;
	isAAC = _isAAC;
	binType = _type;
}

void TraceManager::traceScene(std::string sceneFile)
{
	std::clock_t start;
	double duration;
	std::cout << "Scene file\t: " << sceneFile << std::endl;
	std::cout << "#thread(s)\t: " << tn << std::endl;
	std::cout << "Using AAC?\t: " << isAAC << std::endl;
	std::cout << "Bin type\t: " << binType << std::endl;
	std::cout << "================================" << std::endl;
//	system("pause");
	std::cout << "parsing file\t: ";
	start = std::clock();
	initScene(sceneFile);
	duration = (std::clock() - start) / (double)CLOCKS_PER_SEC;
	std::cout << duration << " s" << std::endl;

	std::cout << "================================" << std::endl;
	std::cout << "image size\t: " << ViewPlane::Instance()->pixelW << " x " << ViewPlane::Instance()->pixelH << std::endl;
	std::cout << "#object(s)\t: " << scene.geometries.size() << std::endl;
	std::cout << "#light(s)\t: " << scene.lights.size() << std::endl;
	std::cout << "max depth\t: " << scene.maxDepth << std::endl;
	std::cout << "================================" << std::endl;
//	system("pause");
	std::cout << "bulding bvh\t: ";
	start = std::clock();
	buildBVH();
	duration = (std::clock() - start) / (double)CLOCKS_PER_SEC;
	std::cout << duration << " s" << std::endl;
//	system("pause");
	std::cout << "tracing scene\t: ";
	start = std::clock();
	FreeImage_Initialise();
	trace();
	duration = (std::clock() - start) / (double)CLOCKS_PER_SEC;
	std::cout << duration << " s" << std::endl;

	mergeAndSaveImage();
	FreeImage_DeInitialise();
	std::cout << "image saved\t: " << outFileName << std::endl;
}

TraceManager::~TraceManager()
{
}

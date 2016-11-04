#include "Tracemanager.h"
#include <ctime>
#include <iostream>
#include <thread>

void TraceManager::initScene(std::string sceneFile) {
	std::clock_t start = std::clock();
	double duration;


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

	duration = (std::clock() - start) / (double)CLOCKS_PER_SEC;
	std::cout << "parsing scene file\t: " << duration << " s" << std::endl;
}

void TraceManager::buildBVH() {
	std::clock_t start = std::clock();
	double duration;


	BVHBuilder(binType, isAAC).BuildBVH(scene);


	duration = (std::clock() - start) / (double)CLOCKS_PER_SEC;
	std::cout << "bulding bvh\t: " << duration << " s" << std::endl;
}

void TraceManager::trace() {
	std::clock_t start = std::clock();
	double duration;
	//DateTime start = DateTime.Now;
	//Console.WriteLine("Tracing...Please Wait...");
	//for (int i = 0; i < tn * 5; i++) Console.Write("-");
	//Console.WriteLine();


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
	duration = (std::clock() - start) / (double)CLOCKS_PER_SEC;
	std::cout << "tracing scene\t: " << duration << " s" << std::endl;
}

void TraceManager::traceThread(FIBITMAP * image, Scene &scene, int rowStart, int colStart, int rowEnd, int colEnd)
{
	for (int currRow = rowStart; currRow < rowEnd; currRow++)
	{
		for (int currCol = colStart; currCol < colEnd; currCol++)
		{
			Ray ray = Ray();
			Point3 pixPosition = ViewPlane::Instance()->getNewLocation(currCol, currRow);
			ray.start = Camera::Instance()->pos;
			ray.direction = Vec3(ray.start, pixPosition).Normalize();

			RayManager().traceRay(ray, *scene.bin);
			RGBQUAD color;
			color.rgbRed = 0;
			color.rgbGreen = 0;
			color.rgbBlue = 0;

			if (ray.intersectWith != nullptr) {
				MyColor& col = RayManager().getColor(ray, scene, scene.maxDepth);
				color = MyColToRGBQUAD(col);
			}

			FreeImage_SetPixelColor(image, currCol, currRow, &color);
		}
	}
}


void TraceManager::mergeAndSaveImage() {
	std::clock_t start = std::clock();
	double duration;

	FreeImage_FlipVertical(image);
	FreeImage_Save(FIF_BMP, image, outFileName.c_str(), 0);
	duration = (std::clock() - start) / (double)CLOCKS_PER_SEC;
	std::cout << "image saved\t: " << outFileName << std::endl;
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

	std::cout << "Scene file\t: " << sceneFile << std::endl;
	std::cout << "#thread(s)\t: " << tn <<std::endl;
	std::cout << "Using AAC?\t: " << isAAC << std::endl;
	std::cout << "Bin type\t: " << binType << std::endl;
	std::cout << "================================" << std::endl;

	initScene(sceneFile);
	std::cout << "================================"<< std::endl;
	std::cout << "image size\t: " << ViewPlane::Instance()->pixelW << " x " << ViewPlane::Instance()->pixelH << std::endl;
	std::cout << "#object(s)\t: " << scene.geometries.size() << std::endl;
	std::cout << "#light(s)\t: " << scene.lights.size() << std::endl;
	std::cout << "max depth\t: " << scene.maxDepth << std::endl;
	std::cout << "================================" << std::endl;
	buildBVH();

	FreeImage_Initialise();
	trace();

	mergeAndSaveImage();
	FreeImage_DeInitialise();

}

TraceManager::~TraceManager()
{
}

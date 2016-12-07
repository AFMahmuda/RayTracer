#include "Tracemanager.h"
#include <ctime>
#include <iostream>
#include <thread>
#include <string>
#include <iomanip>
#include <chrono>
#include <sstream>


#include <fstream> //for writing to file

#include"BVHBuilder.h"
#include"RayManager.h"
#include"Camera.h"
#include"ViewPlane.h"
#include "RadixSort.h"

void TraceManager::initScene(std::string sceneFile) {
	scene = Scene(sceneFile);


	try
	{
		if (tn % 2 == 1)
			throw 0;
		//precalculate w and h measurements
		width = scene.size[0];
		height = scene.size[1];

		//search two closest factors 6 => 3 & 2 ; 5 => 5 & 1
		verDiv = (int)sqrtf(tn) - 1;
		do { verDiv++; if (verDiv > tn) throw 0; } while (tn % verDiv != 0);
		horDiv = tn / verDiv;

		wPerSeg = width / horDiv; //width per segmen
		hPerSeg = height / verDiv; //height per segmen
		outFileName = scene.outFileName;
	}
	catch (int e)
	{
		std::cout << "exception thrown, please check render thread number and size." << std::endl;
		std::exit(EXIT_FAILURE);
	}

}

void TraceManager::buildBVH() {
	BVHBuilder(binType, isAAC, aacThres).BuildBVH(scene);
}

void TraceManager::trace() {
	std::vector<std::thread> threads;
	image = FreeImage_Allocate(width, height, 24);

	//nothing in scene
	if (scene.bin == nullptr)
		return;


	for (int i = 0; i < verDiv; i++)
		for (int j = 0; j < horDiv; j++)
		{
			int row = i, col = j;
			threads.push_back(std::thread(traceThread, image, std::ref(scene), row * hPerSeg, col * wPerSeg, (row + 1) * hPerSeg, (col + 1) * wPerSeg));
		}
	std::for_each(threads.begin(), threads.end(), std::mem_fn(&std::thread::join));

}

void TraceManager::traceThread(FIBITMAP * image, Scene &scene, int rowStart, int colStart, int rowEnd, int colEnd)
{
	RayManager& rayManager = RayManager();
	Ray* ray;
	Vec3 pixPosition;
	RGBQUAD color;
	for (int currRow = rowStart; currRow < rowEnd; currRow++)
	{
		for (int currCol = colStart; currCol < colEnd; currCol++)
		{
			ray = new Ray();
			ray->type = Ray::RAY;
			pixPosition = ViewPlane::Instance()->getNewLocation(currCol, currRow);
			ray->start = Camera::Instance()->pos;
			ray->direction = Vec3(ray->start, pixPosition).normalize();

			rayManager.traceRayIter(*ray, scene.bin);

			if (ray->intersectWith == nullptr) {
				color = MyColToRGBQUAD(MyColor(.2, .2, .2));
			}
			else {
				MyColor& col = RayManager().getColor(*ray, scene, scene.maxDepth);
				color = MyColToRGBQUAD(col);
			}
			FreeImage_SetPixelColor(image, currCol, currRow, &color);
			delete(ray);
		}
	}
}


void TraceManager::mergeAndSaveImage(std::string time) {

	FreeImage_FlipVertical(image);
	FreeImage_Save(FIF_BMP, image, (time + " " + outFileName).c_str(), 0);
}

RGBQUAD TraceManager::MyColToRGBQUAD(MyColor & col)
{
	RGBQUAD color;
	color.rgbRed = col.r * 255;
	color.rgbGreen = col.g * 255;
	color.rgbBlue = col.b * 255;
	return color;
}

TraceManager::TraceManager(int threadNumber, Container::TYPE _type, bool _isAAC, int aacThreshold)
{
	tn = threadNumber;
	isAAC = _isAAC;
	binType = _type;
	aacThres = aacThreshold;
}

void TraceManager::traceScene(std::string sceneFile)
{
	std::ofstream report;
	auto t = std::time(nullptr);
	auto tm = *std::localtime(&t);

	std::ostringstream oss;
	oss << std::put_time(&tm, "%Y%m%d-%H %M %S");
	auto fname = oss.str();
	report.open(fname + ".txt");

	std::clock_t start;
	double duration;
	report << "Scene file\t: " << sceneFile << std::endl;
	std::cout << "Scene file\t: " << sceneFile << std::endl;
	report << "#thread(s)\t: " << tn << std::endl;
	std::cout << "#thread(s)\t: " << tn << std::endl;
	report << "Using AAC?\t: " << isAAC << std::endl;
	std::cout << "Using AAC?\t: " << isAAC << std::endl;
	if (isAAC) report << "AAC threshold\t: " << aacThres << std::endl;
	if (isAAC) std::cout << "AAC threshold\t: " << aacThres << std::endl;
	std::string type = (binType == Container::BOX) ? "BOX" : "SPHERE";
	report << "Bin type\t: " << type << std::endl;
	std::cout << "Bin type\t: " << type << std::endl;
	report << "================================" << std::endl;
	std::cout << "================================" << std::endl;

	report << "parsing file\t: ";
	std::cout << "parsing file\t: ";
	start = std::clock();
	initScene(sceneFile);
	duration = (std::clock() - start) / (double)CLOCKS_PER_SEC * 1000;
	report << duration << " ms" << std::endl;
	std::cout << duration << " ms" << std::endl;


	report << "================================" << std::endl;
	std::cout << "================================" << std::endl;
	report << "image size\t: " << ViewPlane::Instance()->pixelW << " x " << ViewPlane::Instance()->pixelH << std::endl;
	std::cout << "image size\t: " << ViewPlane::Instance()->pixelW << " x " << ViewPlane::Instance()->pixelH << std::endl;
	report << "#object(s)\t: " << scene.geometries.size() << std::endl;
	std::cout << "#object(s)\t: " << scene.geometries.size() << std::endl;
	report << "#light(s)\t: " << scene.lights.size() << std::endl;
	std::cout << "#light(s)\t: " << scene.lights.size() << std::endl;
	report << "max depth\t: " << scene.maxDepth << std::endl;
	std::cout << "max depth\t: " << scene.maxDepth << std::endl;
	report << "================================" << std::endl;
	std::cout << "================================" << std::endl;


	report << "sorting objects\t: ";
	std::cout << "sorting objects\t: ";
	start = std::clock();
	RadixSort::radixsort(scene.geometries);
	duration = (std::clock() - start) / (double)CLOCKS_PER_SEC * 1000;
	report << duration << " ms" << std::endl;
	std::cout << duration << " ms" << std::endl;
	report << "bulding bvh\t: ";
	std::cout << "bulding bvh\t: ";
	start = std::clock();
	buildBVH();
	duration = (std::clock() - start) / (double)CLOCKS_PER_SEC * 1000;
	report << duration << " ms" << std::endl;
	std::cout << duration << " ms" << std::endl;

	report << "tracing scene\t: ";
	std::cout << "tracing scene\t: ";
	start = std::clock();
	FreeImage_Initialise();
	trace();
	duration = (std::clock() - start) / (double)CLOCKS_PER_SEC * 1000;
	report << duration << " ms" << std::endl;
	std::cout << duration << " ms" << std::endl;

	mergeAndSaveImage(fname);
	FreeImage_DeInitialise();
	report << "image saved\t: " << outFileName << std::endl;
	std::cout << "image saved\t: " << outFileName << std::endl;
	report.close();
}

TraceManager::~TraceManager()
{
}

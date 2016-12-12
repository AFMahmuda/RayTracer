#pragma once
#include"Scene.h"


#include"FreeImage.h"

class TraceManager
{

	int tn = 1;
	bool isAAC = false;
	Container::TYPE binType;
	int aacThres;
	Scene scene;
	FIBITMAP * image;
	std::string outFileName;
	int height;
	int width;

	int verDiv;
	int horDiv;

	int wPerSeg;
	int hPerSeg;

	void initScene(std::string sceneFile);
	void buildBVH();
	void trace();
	static void traceThread(int id, FIBITMAP * image, Scene& scene, int rowStart, int colStart, int rowEnd, int colEnd, float& binCounter, float& triCounter);
	static RGBQUAD myColToRGBQUAD(MyColor & col);
	void mergeAndSaveImage(std::string time);
	std::ofstream report;


public:
	TraceManager(int threadNumber = 1, Container::TYPE _type = Container::BOX, bool _isAAC = true, int aacThreshold = 20);
	void traceScene(std::string sceneFile);
	~TraceManager();
};



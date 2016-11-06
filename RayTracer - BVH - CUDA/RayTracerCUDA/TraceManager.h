#pragma once
#include"Scene.h"
#include"BVHBuilder.h"
#include"RayManager.h"

#include"FreeImage.h"

class TraceManager
{

	int tn = 1;
	bool isAAC = false;
	Container::TYPE binType;
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
	static void traceThread(FIBITMAP * image, Scene& scene, int rowStart, int colStart, int rowEnd, int colEnd);
	void mergeAndSaveImage();
	static RGBQUAD MyColToRGBQUAD(MyColor & col);
public:
	TraceManager(int threadNumber = 1, Container::TYPE _type = Container::BOX, bool _isAAC = true);
	void traceScene(std::string sceneFile);
	~TraceManager();
};



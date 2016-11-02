#include "Tracemanager.h"


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
}

void TraceManager::buildBVH() {
	//DateTime start = DateTime.Now;
	//Console.WriteLine("Building BVH. Please Wait...");

	BVHBuilder(binType, isAAC).BuildBVH(scene);
	//Console.WriteLine("DONE! " + (DateTime.Now - start) + "\n");

}

void TraceManager::trace() {
	//DateTime start = DateTime.Now;
	//Console.WriteLine("Tracing...Please Wait...");
	//for (int i = 0; i < tn * 5; i++) Console.Write("-");
	//Console.WriteLine();

	//Task[] traceTask = new Task[tn];

	//results = new Bitmap[tn];
	//int cnt = 0;
	//for (int i = 0; i < verDiv; i++)
	//	for (int j = 0; j < horDiv; j++)
	//	{
	//		results[cnt] = new Bitmap(wPerSeg, hPerSeg);

	//		int row = i, col = j, n = cnt;
	//		traceTask[cnt] = Task.Factory.StartNew(() = >
	//			TraceThread(results[n], scene, row * hPerSeg, col * wPerSeg, (row + 1) * hPerSeg, (col + 1) * wPerSeg)
	//			);
	//		cnt++;
	//	}

	//Task.WaitAll(traceTask);
	//Console.Write("\n");
	//Console.WriteLine("DONE! " + (DateTime.Now - start) + "\n");
	traceThread(scene, 0, 0, height, width);
}

void TraceManager::traceThread(Scene & scene, int rowStart, int colStart, int rowEnd, int colEnd)
{
	//float segmen = (colEnd - colStart) * (rowEnd - rowStart) / 5f;
	//float count = 0;

	image = FreeImage_Allocate(width, height, 24);
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
			color.rgbRed = (currCol / (float)colEnd) * 255;
			color.rgbGreen = (currRow / (float)rowEnd) * 255;
			color.rgbBlue = .5 * 255;

			if (ray.intersectWith != nullptr) {
				MyColor& col = RayManager().getColor(ray, scene, scene.maxDepth);
				color.rgbRed = col.r * 255;
				color.rgbGreen = col.g * 255;
				color.rgbBlue = col.b * 255;
			}

			FreeImage_SetPixelColor(image, currCol, rowEnd - currRow, &color);
			//MyColor rayColor = ray.GetColor(scene, scene.MaxDepth);

			//result.SetPixel(currCol - colStart, currRow - rowStart, rayColor.ToColor());

			//progress bar

			/*if (++count >= segmen)
			{
			Console.Write("*");
			count -= (segmen);
			}*/
		}
	}

}

void TraceManager::mergeAndSaveImage() {
	/*string filename = scene.OutputFilename;
	int cnt = 0;
	Bitmap res = new Bitmap(width, height);
	using (Graphics finalResult = Graphics.FromImage(res))
	{
	int tempW = 0, tempH = 0;
	for (int i = 0; i < verDiv; i++)
	{
	for (int j = 0; j < horDiv; j++)
	{
	finalResult.DrawImage(results[cnt], tempW, tempH);
	tempW += results[cnt].Width;
	cnt++;
	}
	tempW = 0;
	tempH += results[cnt - 1].Height;
	}
	}

	if (filename.Equals(""))
	filename = "default.bmp";
	res.Save(filename);
	Console.WriteLine("Saved in : " + Directory.GetCurrentDirectory() + "\\" + filename + "\n");*/
	FreeImage_Save(FIF_BMP, image, "default.bmp", 0);
}

TraceManager::TraceManager(int threadNumber, Container::TYPE _type, bool _isAAC)//, Container.TYPE _type = Container.TYPE.BOX)
{
	tn = threadNumber;
	isAAC = _isAAC;
	binType = _type;
}

void TraceManager::traceScene(std::string sceneFile)
{
	initScene(sceneFile);

	buildBVH();

	FreeImage_Initialise();
	trace();

	mergeAndSaveImage();
	FreeImage_DeInitialise();

}

TraceManager::~TraceManager()
{
}

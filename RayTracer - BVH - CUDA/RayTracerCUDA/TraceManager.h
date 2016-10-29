#pragma once
#include<string>
#include"Scene.h"
#include"Container.h"

#include"ViewPlane.h"
#include<cmath>

class TraceManager
{

	int tn = 4;
	bool isAAC = false;
	//	Container::TYPE binType;
	Scene scene;
	//	Bitmap image;

	int height;
	int width;

	int verDiv;
	int horDiv;

	int wPerSeg;
	int hPerSeg;
public:

	TraceManager(int threadNumber = 4, bool _isAAC = true)//, Container.TYPE _type = Container.TYPE.BOX)
	{
		tn = threadNumber;
		isAAC = _isAAC;
		//		type = _type;
	}

	void traceScene(std::string sceneFile)
	{
		initScene(sceneFile);

		buildBVH();

		trace();

		mergeAndSaveImage();
	}

	void initScene(std::string sceneFile) {
		scene = Scene(sceneFile);
		//precalculate w and h measurements
		//w and h total
		height = ViewPlane::Instance().pixelH;
		width = ViewPlane::Instance().pixelW;

		//search two closest factors 6 = 3 and 2 , 5 = 5 and 1
		verDiv = (int)sqrtf(tn);
		do verDiv++; while (tn % verDiv != 0);
		horDiv = tn / verDiv;

		wPerSeg = width / horDiv; //width per segmen
		hPerSeg = height / verDiv; //height per segmen
	}

	void 	buildBVH() {
		//DateTime start = DateTime.Now;
		//Console.WriteLine("Building BVH. Please Wait...");
		//new BVHManager(type, isAAC).BuildBVH(scene);
		//Console.WriteLine("DONE! " + (DateTime.Now - start) + "\n");

	}

	void 	trace() {
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
	}

	void traceThread()
	{
		//float segmen = (colEnd - colStart) * (rowEnd - rowStart) / 5f;
		//float count = 0;

		//for (int currRow = rowStart; currRow < rowEnd; currRow++)
		//{
		//	for (int currCol = colStart; currCol < colEnd; currCol++)
		//	{
		//		Ray ray = new Ray();
		//		Point3 pixPosition = ViewPlane.Instance.GetNewLocation(currCol, currRow);
		//		ray.Start = Camera.Instance.Position;
		//		ray.Direction = new Vec3(ray.Start, pixPosition).Normalize();

		//		ray.Trace(scene, scene.Bvh);
		//		MyColor rayColor = ray.GetColor(scene, scene.MaxDepth);

		//		result.SetPixel(currCol - colStart, currRow - rowStart, rayColor.ToColor());

		//		//progress bar

		//		if (++count >= segmen)
		//		{
		//			Console.Write("*");
		//			count -= (segmen);
		//		}
		//	}
		//}
	}

	void	mergeAndSaveImage() {
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
	}


	TraceManager();


	~TraceManager();
};


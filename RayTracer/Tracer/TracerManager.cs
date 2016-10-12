using RayTracer.BVH;
using RayTracer.Common;
using RayTracer.Material;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;



namespace RayTracer.Tracer
{

    public class TracerManager
    {
        int tn;        //thread number
        bool isAAC;
        Container.TYPE type;
        Scene scene;
        Bitmap[] results;

        int verDiv;
        int horDiv;

        int height;
        int width;

        int wPerSeg;
        int hPerSeg;

        public TracerManager(int threadNumber = 4, bool _isAAC = true, Container.TYPE _type = Container.TYPE.BOX)
        {
            tn = threadNumber;
            isAAC = _isAAC;
            type = _type;
        }

        public void TraceScene(string sceneFile)
        {

            //init scene[0] and copy to scene [1 to (tn-1)]
            initScene(sceneFile);


            //build bvh for scene[0] and copy to scene [1 to (tn-1)]
            BuildBVH();

            //trace scene using multi thread
            Trace();

            //merge images from traced scenes
            MergeAndSaveImage();

        }

        void initScene(string sceneFile)
        {
            DateTime start = DateTime.Now;
            Console.WriteLine("Preparing Scene. Please Wait...");

            scene = new Scene(sceneFile);
            Console.WriteLine("DONE! " + (DateTime.Now - start) + "\n");

            scene.ShowInformation();


            //precalculate w and h measurements
            //w and h total
            height = ViewPlane.Instance.PixelHeight;
            width = ViewPlane.Instance.PixelWidth;

            //search two closest factors 6 = 3 and 2 , 5 = 5 and 1
            verDiv = (int)Math.Sqrt(tn);
            do verDiv++; while (tn % verDiv != 0);
            horDiv = tn / verDiv;

            wPerSeg = width / horDiv; //width per segmen
            hPerSeg = height / verDiv; //height per segmen

        }

        void BuildBVH()
        {
            DateTime start = DateTime.Now;
            Console.WriteLine("Building BVH. Please Wait...");
            new BVHManager(type, isAAC).BuildBVH(scene);
            Console.WriteLine("DONE! " + (DateTime.Now - start) + "\n");
        }

        void Trace()
        {
            DateTime start = DateTime.Now;
            Console.WriteLine("Tracing...Please Wait...");
            for (int i = 0; i < tn * 5; i++) Console.Write("-");
            Console.WriteLine();

            Task[] traceTask = new Task[tn];

            results = new Bitmap[tn];
            int cnt = 0;
            for (int i = 0; i < verDiv; i++)
                for (int j = 0; j < horDiv; j++)
                {
                    results[cnt] = new Bitmap(wPerSeg, hPerSeg);

                    int row = i, col = j, n = cnt;
                    traceTask[cnt] = Task.Factory.StartNew(() =>
                        TraceThread(results[n], scene, row * hPerSeg, col * wPerSeg, (row + 1) * hPerSeg, (col + 1) * wPerSeg)
                    );
                    cnt++;
                }

            Task.WaitAll(traceTask);
            Console.Write("\n");
            Console.WriteLine("DONE! " + (DateTime.Now - start) + "\n");
        }

        void TraceThread(Bitmap result, Scene scene, int rowStart, int colStart, int rowEnd, int colEnd)
        {
            float segmen = (colEnd - colStart) * (rowEnd - rowStart) / 5f;
            float count = 0;

            for (int currRow = rowStart; currRow < rowEnd; currRow++)
            {
                for (int currCol = colStart; currCol < colEnd; currCol++)
                {
                    Ray ray = new Ray();
                    Point3 pixPosition = ViewPlane.Instance.GetNewLocation(currCol, currRow);
                    ray.Start = Camera.Instance.Position;
                    ray.Direction = new Vec3(ray.Start, pixPosition).Normalize();

                    ray.Trace(scene, scene.Bvh);
                    MyColor rayColor = ray.GetColor(scene, scene.MaxDepth);

                    result.SetPixel(currCol - colStart, currRow - rowStart, rayColor.ToColor());

                    //progress bar

                    if (++count >= segmen)
                    {
                        Console.Write("*");
                        count -= (segmen);
                    }
                }
            }
        }


        void MergeAndSaveImage()
        {
            string filename = scene.OutputFilename;
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
            Console.WriteLine("Saved in : " + Directory.GetCurrentDirectory() + "\\" + filename + "\n");
        }
    }
}

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
        Scene[] scenes;
        Bitmap[] results;
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

        void Trace()
        {
            results = new Bitmap[tn];
            int h = ViewPlane.Instance.PixelHeight;
            int w = ViewPlane.Instance.PixelWidth;
            for (int i = 0; i < tn; i++)
                results[i] = new Bitmap(w, h);

            DateTime start = DateTime.Now;
            Console.WriteLine("Tracing...Please Wait...");
            for (int i = 0; i < tn * 5; i++) Console.Write("-");
            Console.WriteLine();

            Task[] traceTask = new Task[tn];
            int vDiv = 2;
            int hDiv = tn / vDiv;
            int cnt = 0;
            for (int i = 0; i < vDiv; i++)
                for (int j = 0; j < hDiv; j++)
                {
                    int row = i, col = j, n = cnt;
                    traceTask[cnt] = Task.Factory.StartNew(() =>
                        TraceThread(results[n], scenes[n], row * h / vDiv, col * w / hDiv, (row + 1) * h / vDiv, (col + 1) * w / hDiv)
                    );
                    cnt++;
                }

            Task.WaitAll(traceTask);
            Console.Write("\n");
            Console.WriteLine("DONE! " + (DateTime.Now - start) + "\n");
        }

        //init scene[0] and copy to scene [1-(tn-1)]
        Scene[] initScene(string sceneFile)
        {
            DateTime start = DateTime.Now;
            Console.WriteLine("Preparing Scene. Please Wait...");

            scenes = new Scene[tn];
            Task[] sceneTask = new Task[tn - 1];
            scenes[0] = new Scene(sceneFile);

            for (int i = 0; i < tn - 1; i++)
            {
                int j = i;
                sceneTask[i] = Task.Factory.StartNew(() => scenes[j + 1] = Utils.DeepClone(scenes[0]));
            }
            Task.WaitAll(sceneTask);
            Console.WriteLine("DONE! " + (DateTime.Now - start) + "\n");

            scenes[0].ShowInformation();
            return scenes;
        }

        void BuildBVH()
        {
            DateTime start = DateTime.Now;
            Console.WriteLine("Building BVH. Please Wait...");
            new BVHManager(type, isAAC).BuildBVH(scenes[0]);
            Task[] bvhThread = new Task[tn - 1];
            for (int i = 0; i < tn - 1; i++)
            {
                int j = i;
                bvhThread[i] = Task.Factory.StartNew(() => scenes[j + 1].Bvh = Utils.DeepClone(scenes[0].Bvh));
            }
            Task.WaitAll(bvhThread);
            Console.WriteLine("DONE! " + (DateTime.Now - start) + "\n");
        }

        void MergeAndSaveImage()
        {
            string filename = scenes[0].OutputFilename;
            using (Graphics finalResult = Graphics.FromImage(results[0]))
            {
                for (int i = 1; i < results.Length; i++)
                    finalResult.DrawImage(results[i], 0, 0);
            }

            if (filename.Equals(""))
                filename = "default.bmp";
            results[0].Save(filename);
            Console.WriteLine("Saved in : " + Directory.GetCurrentDirectory() + "\\" + filename + "\n");
        }


        void TraceThread(Bitmap result, Scene scene, int rowStart, int colStart, int rowEnd, int colEnd)
        {
            float total = (colEnd - colStart) * (rowEnd - rowStart);
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

                    result.SetPixel(currCol, currRow, rayColor.ToColor());

                    //progress bar
                    if (++count >= total / 5.0)
                    {
                        Console.Write("*");
                        count = count - (total / 5.0f);
                    }
                }
            }
        }
    }
}

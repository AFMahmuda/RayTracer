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
        int tn = 6;

        public bool TraceScene(string sceneFile)
        {


            DateTime start = DateTime.Now;
            Console.WriteLine("Preparing Scene. Please Wait...");

            Scene[] scene = InitScenes(sceneFile);
            scene[0].ShowInformation();

            Console.WriteLine("DONE! " + (DateTime.Now - start) + "\n");

            Bitmap[] results = new Bitmap[tn];
            int h = ViewPlane.Instance.PixelHeight;
            int w = ViewPlane.Instance.PixelWidth;

            for (int i = 0; i < tn; i++)
                results[i] = new Bitmap(w, h);
            start = DateTime.Now;
            Console.WriteLine("Tracing...Please Wait...\n------------------------------");

            Task[] traceTask = new Task[6];


            int num = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int ii = i;
                    int jj = j;
                    int n = num;

                    traceTask[num] = Task.Factory.StartNew(() =>
                        TraceThread(results[n], scene[n], ii * h / 2, jj * w / 3, (ii + 1) * h / 2, (jj + 1) * w / 3)
                    );
                    num++;
                }

            }

            Task.WaitAll(traceTask);
            Console.Write("\n");

            MergeAndSaveImage(results, scene[0].OutputFilename);

            Console.WriteLine("Finised in :" + (DateTime.Now - start) );
            return true;
        }

        Scene[] InitScenes(string sceneFile)
        {

            Scene[] scene = new Scene[tn];
            Task[] sceneTask = new Task[tn - 1];
            scene[0] = new Scene(sceneFile);

            for (int i = 0; i < tn - 1; i++)
            {
                int j = i;
                sceneTask[i] = Task.Factory.StartNew(() => scene[j + 1] = Utils.DeepClone(scene[0]));
            }
            Task.WaitAll(sceneTask);
            return scene;
        }

        void MergeAndSaveImage(Bitmap[] result, string filename)
        {

            using (Graphics finalResult = Graphics.FromImage(result[0]))
            {
                for (int i = 1; i < result.Length; i++)
                    finalResult.DrawImage(result[i], 0, 0);
            }

            if (filename.Equals(""))
                filename = "default.bmp";
            result[0].Save(filename);

            Console.WriteLine("Saved in : " + Directory.GetCurrentDirectory() + "\\" + filename+"\n");
        }

        void showTree(Container bin, int level = 1)
        {
            Console.Write("lv : " + level + " ");
            ((SphereContainer)bin).ShowInformation();
            foreach (Container item in bin.Childs)
            {
                showTree(item, level + 1);
            }
        }

        void TraceThread(Bitmap result, Scene scene, int rowStart, int colStart, int rowEnd, int colEnd)
        {
            new BVHManager().BuildBVH(scene);

            double total = (colEnd - colStart) * (rowEnd - rowStart);
            double count = 0;
            for (int currRow = rowStart; currRow < rowEnd; currRow++)
            {
                for (int currCol = colStart; currCol < colEnd; currCol++)
                {
                    Ray ray = new Ray();
                    Point3 pixPosition = ViewPlane.Instance.GetNewLocation(currCol, currRow);
                    ray.Start = Camera.Instance.Position;
                    ray.Direction = new Vector3(ray.Start, pixPosition).Normalize();

                    ray.Trace(scene, scene.Bvh);
                    MyColor rayColor = ray.GetColor(scene, scene.MaxDepth);

                    result.SetPixel(currCol, currRow, rayColor.ToColor());

                    //progress bar
                    if (++count >= total / 5.0)
                    {
                        Console.Write("*");
                        count = count - (total / 5.0);
                    }
                }
            }
        }
    }
}

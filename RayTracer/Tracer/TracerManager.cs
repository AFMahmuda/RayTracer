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
        //thread number
        int tn = 6;

        public bool TraceScene(string sceneFile)
        {
            DateTime start = DateTime.Now;
            Console.WriteLine("Preparing Scene. Please Wait...");

            //init scene[0] and copy to scene [1-(tn-1)]
            Scene[] scene = InitScenes(sceneFile);
            scene[0].ShowInformation();

            Console.WriteLine("DONE! " + (DateTime.Now - start));

            Bitmap[] results = new Bitmap[tn];
            int h = ViewPlane.Instance.PixelHeight;
            int w = ViewPlane.Instance.PixelWidth;

            for (int i = 0; i < tn; i++)
                results[i] = new Bitmap(w, h);
            start = DateTime.Now;
            Console.WriteLine("Tracing...Please Wait...\n");
            for (int i = 0; i < tn*5; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();

            Task[] traceTask = new Task[tn];

            int vDiv = 2;
            int hDiv = tn / vDiv;

            int cnt = 0;
            for (int i = 0; i < vDiv; i++)
            {
                for (int j = 0; j < hDiv; j++)
                {
                    int ii = i;
                    int jj = j;
                    int n = cnt;

                    traceTask[cnt] = Task.Factory.StartNew(() =>
                        TraceThread(results[n], scene[n], ii * h / vDiv, jj * w / hDiv, (ii + 1) * h / vDiv, (jj + 1) * w / hDiv)
                    );
                    cnt++;
                }

            }

            Task.WaitAll(traceTask);
            Console.Write("\n");

            MergeAndSaveImage(results, scene[0].OutputFilename);
            Console.WriteLine("Saved in : " + Directory.GetCurrentDirectory() + "\\" + scene[0].OutputFilename + "\n");

            Console.WriteLine("Finised in :" + (DateTime.Now - start));
            return true;
        }

        //init scene[0] and copy to scene [1-(tn-1)]
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

        }


        void TraceThread(Bitmap result, Scene scene, int rowStart, int colStart, int rowEnd, int colEnd)
        {
            new BVHManager().BuildBVH(scene);

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

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

        public bool TraceScene(string sceneFile)
        {


            DateTime start = DateTime.Now;
            Console.WriteLine("Preparing Scene. Please Wait...");

            Scene[] scene = new Scene[6];
            Task[] sceneTask = new Task[5];
            scene[0] = new Scene(sceneFile);

            for (int i = 0; i < 5; i++)
            {
//                scene[i] = new Scene();
                int j = i;
                //                sceneTask[i] = Task.Factory.StartNew(() => scene[j].ParseCommand(sceneFile));
                sceneTask[i] = Task.Factory.StartNew(() => scene[j+1] = Utils.DeepClone(scene[0]));
            }
            Task.WaitAll(sceneTask);
            scene[0].ShowInformation();

            Console.WriteLine("DONE! " + (DateTime.Now - start) + "\n");


            int h = ViewPlane.Instance.PixelHeight;
            int w = ViewPlane.Instance.PixelWidth;

            Bitmap[] results = new Bitmap[6];
            for (int i = 0; i < 6; i++)

                results[i] = new Bitmap(w, h);

            start = DateTime.Now;
            Console.WriteLine("Tracing...Please Wait...\n------------------------------");

            Task[] traceTask = new Task[6];


            traceTask[0] = Task.Factory.StartNew(() => TraceThread(results[0], scene[0], 0, 0, h / 2, w / 3));
            traceTask[1] = Task.Factory.StartNew(() => TraceThread(results[1], scene[1], 0, w / 3, h / 2, w * 2 / 3));
            traceTask[2] = Task.Factory.StartNew(() => TraceThread(results[2], scene[2], 0, w * 2 / 3, h / 2, w));
            traceTask[3] = Task.Factory.StartNew(() => TraceThread(results[3], scene[3], h / 2, 0, h, w / 3));
            traceTask[4] = Task.Factory.StartNew(() => TraceThread(results[4], scene[4], h / 2, w / 3, h, w * 2 / 3));
            traceTask[5] = Task.Factory.StartNew(() => TraceThread(results[5], scene[5], h / 2, w * 2 / 3, h, w));

            Task.WaitAll(traceTask);
            //Console.WriteLine();
            //showTree(scene.Bvh);


            MergeAndSaveImage(results, scene[0].OutputFilename);

            Console.WriteLine("\nFinised in :" + (DateTime.Now - start) + "\n");

            return true;

        }

        void MergeAndSaveImage(Bitmap[] result, string filename)
        {

            using (Graphics finalResult = Graphics.FromImage(result[0]))
            {
                for (int i = 1; i < 6; i++)
                    finalResult.DrawImage(result[i], 0, 0);
            }

            if (filename.Equals(""))
                filename = "default.bmp";
            result[0].Save(filename);

            Console.WriteLine("Saved in : " + Directory.GetCurrentDirectory() + "\\" + filename);
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

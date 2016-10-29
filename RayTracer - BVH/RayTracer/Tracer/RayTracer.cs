using RayTracer.BVH;
using RayTracer.Common;
using System;
using System.Drawing;
using System.Threading.Tasks;



namespace RayTracer.Tracer
{

    public class RayTraceManager
    {

        public Bitmap TraceScene(Scene scene)
        {
            scene.ShowInformation();

            ViewPlane.Instance = new ViewPlane(scene.Size.Width, scene.Size.Height);

            DateTime start = DateTime.Now;
            Console.WriteLine("Preparing Scene. Please Wait...");

            Scene scene2 = new Scene();
            Scene scene3 = new Scene();
            Scene scene4 = new Scene();
            Scene scene5 = new Scene();
            Scene scene6 = new Scene();

            Task taskScene2 = Task.Factory.StartNew(() => scene2.ParseCommand(scene.SceneFile));
            Task taskScene3 = Task.Factory.StartNew(() => scene3.ParseCommand(scene.SceneFile));
            Task taskScene4 = Task.Factory.StartNew(() => scene4.ParseCommand(scene.SceneFile));
            Task taskScene5 = Task.Factory.StartNew(() => scene5.ParseCommand(scene.SceneFile));
            Task taskScene6 = Task.Factory.StartNew(() => scene6.ParseCommand(scene.SceneFile));
            Task.WaitAll(taskScene2, taskScene3, taskScene4, taskScene5, taskScene6);

            Console.WriteLine("DONE! " + (DateTime.Now - start) + "\n");


            int h = ViewPlane.Instance.PixelHeight;
            int w = ViewPlane.Instance.PixelWidth;

            Bitmap[] results = new Bitmap[6];
            for (int i = 0; i < 6; i++)

                results[i] = new Bitmap(w, h);

            start = DateTime.Now;
            Console.WriteLine("Tracing...Please Wait...\n------------------------------");

            Task task1 = Task.Factory.StartNew(() => TraceThread(results[0], scene, 0, 0, h / 2, w / 3));
            Task task2 = Task.Factory.StartNew(() => TraceThread(results[1], scene2, 0, w / 3, h / 2, w * 2 / 3));
            Task task3 = Task.Factory.StartNew(() => TraceThread(results[2], scene3, 0, w * 2 / 3, h / 2, w));
            Task task4 = Task.Factory.StartNew(() => TraceThread(results[3], scene4, h / 2, 0, h, w / 3));
            Task task5 = Task.Factory.StartNew(() => TraceThread(results[4], scene5, h / 2, w / 3, h, w * 2 / 3));
            Task task6 = Task.Factory.StartNew(() => TraceThread(results[5], scene6, h / 2, w * 2 / 3, h, w));
            Task.WaitAll(task1, task2, task3, task4, task5, task6);


            using (Graphics finalResult = Graphics.FromImage(results[0]))
            {
                for (int i = 1; i < 6; i++)

                    finalResult.DrawImage(results[i], 0, 0);

            }
            Console.WriteLine("\nFinised in :" + (DateTime.Now - start) + "\n");


            return results[0];
        }


        void TraceThread(Bitmap result, Scene scene, int rowStart, int colStart, int rowEnd, int colEnd)
        {
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
                    MyColor rayColor = ray.Trace(scene);

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

        /* superSampling
        void TraceThreadSuperSampling(Bitmap result, Scene scene, int rowStart, int colStart, int rowEnd, int colEnd)
        {
            double total = (colEnd - colStart) * (rowEnd - rowStart);
            double count = 0;
            for (int row = rowStart; row < rowEnd; row++)
            {
                MyColor tempColor = new MyColor();
                for (int col = colStart; col < colEnd; col++)
                {
                    Ray ray;
                    Point3 newPosition;
                    MyColor rayColor = new MyColor();



                    if (col == colStart)
                    {
                        for (int i = -1; i <= 1; i++)
                        {
                            ray = new Ray();
                            ray.Start = Camera.Instance.Position;
                            newPosition = ViewPlane.Instance.GetNewLocation(col, row, .5f, .5f * i);
                            ray.Direction = new Vector3(ray.Start, newPosition).Normalize();
                            tempColor += ray.Trace(scene, 0) * (1f / 9f);
                        }
                    }

                    rayColor += tempColor;
                    tempColor = new MyColor();
                    for (int i = -1; i <= 1; i++)
                        for (int j = 0; j <= 1; j++)
                        {

                            ray = new Ray();
                            ray.Start = Camera.Instance.Position;
                            newPosition = ViewPlane.Instance.GetNewLocation(col, row, .5f * i, .5f * j);
                            ray.Direction = new Vector3(ray.Start, newPosition).Normalize();
                            MyColor currentColor = ray.Trace(scene, 0) * (1f / 9f);
                            if (j == 1)
                                tempColor += currentColor;
                            rayColor += currentColor;
                        }

                    result.SetPixel(col, row, rayColor.ToColor());

                    if (++count >= total / 5f)
                    {
                        Console.Write("*");
                        count = count - (total / 5f);
                    }
                }
            }

        }
        */

    }
}

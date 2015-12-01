using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace RayTracer
{

    public class RayTracer
    {


        public Bitmap TraceScene(Scene scene)
        {

            scene.ShowInformation();

            ViewPlane viewPlane;
            viewPlane = new ViewPlane(scene.Size.Width, scene.Size.Height, scene.Camera);

            DateTime start = DateTime.Now;
            Console.WriteLine("Preparing Tracer. Please Wait...");

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
            start = DateTime.Now;

            int h = viewPlane.PixelHeight;
            int w = viewPlane.PixelWidth;

            Bitmap result = new Bitmap(w, h);
            Bitmap result2 = new Bitmap(w, h);
            Bitmap result3 = new Bitmap(w, h);
            Bitmap result4 = new Bitmap(w, h);
            Bitmap result5 = new Bitmap(w, h);
            Bitmap result6 = new Bitmap(w, h);
            Console.WriteLine("Tracing...Please Wait...\n------------------------------");

            Task task1 = Task.Factory.StartNew(() => TraceThread(result, scene, viewPlane, 0, 0, h / 2, w / 3));
            Task task2 = Task.Factory.StartNew(() => TraceThread(result2, scene2, viewPlane, 0, w / 3, h / 2, w * 2 / 3));
            Task task3 = Task.Factory.StartNew(() => TraceThread(result3, scene3, viewPlane, 0, w * 2 / 3, h / 2, w));
            Task task4 = Task.Factory.StartNew(() => TraceThread(result4, scene4, viewPlane, h / 2, 0, h, w / 3));
            Task task5 = Task.Factory.StartNew(() => TraceThread(result5, scene5, viewPlane, h / 2, w / 3, h, w * 2 / 3));
            Task task6 = Task.Factory.StartNew(() => TraceThread(result6, scene6, viewPlane, h / 2, w * 2 / 3, h, w));
            Task.WaitAll(task1, task2, task3, task4, task5, task6);


            using (Graphics finalResult = Graphics.FromImage(result))
            {
                finalResult.DrawImage(result2, 0, 0);
                finalResult.DrawImage(result3, 0, 0);
                finalResult.DrawImage(result4, 0, 0);
                finalResult.DrawImage(result5, 0, 0);
                finalResult.DrawImage(result6, 0, 0);
            }

            Console.WriteLine("\nFinised in :" + (DateTime.Now - start) + "\n");

            return result;
        }
         

        void TraceThread(Bitmap result, Scene scene, ViewPlane viewPlane, int rowStart, int colStart, int rowEnd, int colEnd)
        {
            Double total = (colEnd - colStart) * (rowEnd - rowStart);
            Double count = 0;
            for (int row = rowStart; row < rowEnd; row++)
            {
                MyColor tempColor = new MyColor();
                for (int col = colStart; col < colEnd; col++)
                {
                    Ray ray;
                    Point3 newPosition;
                    MyColor rayColor = new MyColor();

                    ray = new Ray();
                    ray.Start = scene.Camera.Position;
                    newPosition = viewPlane.GetNewLocation(col, row);
                    ray.Direction = new Vector3(ray.Start, newPosition).Normalize();
                    rayColor = ray.Trace(scene, 0);

                    result.SetPixel(col, row, rayColor.ToColor());

                    if (++count >= total / 5.0)
                    {
                        Console.Write("*");
                        count = count - (total / 5.0);
                    }
                }
            }

        }


        void TraceThreadSuperSampling(Bitmap result, Scene scene, ViewPlane viewPlane, int rowStart, int colStart, int rowEnd, int colEnd)
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
                            ray.Start = scene.Camera.Position;
                            newPosition = viewPlane.GetNewLocation(col, row, .5f, .5f * i);
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
                            ray.Start = scene.Camera.Position;
                            newPosition = viewPlane.GetNewLocation(col, row, .5f * i, .5f * j);
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

        internal Bitmap TraceScene3D(Scene scene)
        {


            Bitmap result = new Bitmap(scene.Size.Width * 2 + 1, scene.Size.Height);

            Scene sceneLeft = Utils.DeepClone(scene);
            sceneLeft.Camera.Position += sceneLeft.Camera.U.Value * .1f;
            Scene sceneRight = Utils.DeepClone(scene);
            sceneRight.Camera.Position += sceneRight.Camera.U.Value * -.1f;



            Bitmap imageLeft = TraceScene(sceneLeft);
            Bitmap imageRight = TraceScene(sceneRight);


            using (Graphics grfx = Graphics.FromImage(result))
            {
                grfx.DrawImage(imageLeft, 0, 0);
                grfx.DrawImage(imageRight, scene.Size.Width + 1, 0);
            }

            return result;


        }
    }
}

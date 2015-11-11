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
        int total;
        int count;

        public Bitmap TraceScene(Scene scene)
        {
            ViewPlane viewPlane;
            Bitmap result;
            Point3 startPosition;

            viewPlane = new ViewPlane(scene.Size.Width, scene.Size.Height, scene.Camera);
            result = new Bitmap(viewPlane.PixelWidth, viewPlane.PixelHeight);
            startPosition = scene.Camera.Position;

            total = viewPlane.PixelWidth * viewPlane.PixelHeight;
            count = 0;

            DateTime start = DateTime.Now;
            Console.WriteLine("--------------------");


            int h = viewPlane.PixelHeight;
            int w = viewPlane.PixelWidth;
            Bitmap result1 = new Bitmap(w, h);
            Bitmap result2 = new Bitmap(w, h);
            Bitmap result3 = new Bitmap(w, h);
            Bitmap result4 = new Bitmap(w, h);

            Task task1 = Task.Factory.StartNew(() => TraceThread(result1, Utils.DeepClone(scene), viewPlane, 0, 0, h / 2, w / 2));
            Task task2 = Task.Factory.StartNew(() => TraceThread(result2, Utils.DeepClone(scene), viewPlane, 0, w / 2, h / 2, w));
            Task task3 = Task.Factory.StartNew(() => TraceThread(result3, Utils.DeepClone(scene), viewPlane, h / 2, 0, h, w / 2));
            Task task4 = Task.Factory.StartNew(() => TraceThread(result4, Utils.DeepClone(scene), viewPlane, h / 2, w / 2, h, w));

            Task.WaitAll(task1, task2, task3, task4);

            using (Graphics finalResult = Graphics.FromImage(result))
            {
                finalResult.DrawImage(result1, 0, 0);
                finalResult.DrawImage(result2, 0, 0);
                finalResult.DrawImage(result3, 0, 0);
                finalResult.DrawImage(result4, 0, 0);
            }

            Console.WriteLine("\nfinised in :" + (DateTime.Now - start));
            return result;
        }


        private object lockObject = new object();


        void TraceThread(Bitmap result, Scene scene, ViewPlane viewPlane, int rowStart, int colStart, int rowEnd, int colEnd)
        {
            for (int r = rowStart; r < rowEnd; r++)
            {
                for (int c = colStart; c < colEnd; c++)
                {
                    Ray ray = new Ray();
                    ray.Start = scene.Camera.Position;
                    Point3 newPosition = viewPlane.GetNewLocation(c, r);
                    ray.Direction = new Vector3(ray.Start, newPosition).Normalize();
                    MyColor rayColor = ray.Trace(scene, 0);

                    result.SetPixel(c, r, rayColor.ToColor());

                    if (++count == total / 20)
                    {
                        Console.Write("*");
                        count = 0;
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

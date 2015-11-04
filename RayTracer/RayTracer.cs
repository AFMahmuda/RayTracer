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
        ViewPlane viewPlane;
        Bitmap result;
        Point3 startPosition;

        int total;
        int count;

        public Bitmap TraceScene(Scene scene)
        {

            viewPlane = new ViewPlane(scene.Size.Width, scene.Size.Height, scene.Camera);
            result = new Bitmap(viewPlane.PixelWidth, viewPlane.PixelHeight);
            startPosition = scene.Camera.Position;

            total = viewPlane.PixelWidth * viewPlane.PixelHeight;
            count = 0;

            DateTime start = DateTime.Now;
            Console.WriteLine("--------------------");

            
            for (int row = 0; row < viewPlane.PixelHeight; row++)
            {
                MyColor[] tempColor = new MyColor[3];

                Ray temp = new Ray();
                temp.Start = startPosition;

                for (int i = -1; i < 2; i++)
                {
                    Point3 tempDir = viewPlane.GetNewLocation(0, row, .5f, i * .5f);
                    temp.Direction = new Vector3(temp.Start, tempDir).Normalize();
                    tempColor[i + 1] = temp.Trace(scene, 0) * (1f / 9f);
                }

                for (int col = 0; col < viewPlane.PixelWidth; col++)
                {
                    MyColor color = new MyColor();

                    Ray ray = new Ray();
                    ray.Start = startPosition;
                    //Point3 newPosition = viewPlane.GetNewLocation(col, row);
                    //ray.Direction = new Vector3(ray.Start, newPosition).Normalize();

                    //color += ray.Trace(scene, 0);


                    color += tempColor[0];
                    color += tempColor[1];
                    color += tempColor[2];

                    for (int i = 0; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            Point3 newPosition = viewPlane.GetNewLocation(col, row, i * .5f, j * .5f);
                            ray.Direction = new Vector3(ray.Start, newPosition).Normalize();
                            MyColor rayColor = ray.Trace(scene, 0) * (1f / 9f);
                            color += rayColor;

                            if (i == 1)
                                tempColor[j + 1] = rayColor; 
                        }
                    }

                    result.SetPixel(col, row, color.ToColor());



                    if (++count == total / 20)
                    {
                        Console.Write("*");
                        count = 0;
                    }
                }

            }

            Console.WriteLine();

            Console.WriteLine("finised in :" + (DateTime.Now - start));
            return result;
        }


        //private object lockObject = new object();

        //void TracePixel(Scene scene, Ray ray, int row, int col)
        //{
        //    MyColor color = ray.Trace(scene, 0);
        //    lock (lockObject)
        //        result.SetPixel(col, row, color.ToColor());
        //}

    }
}

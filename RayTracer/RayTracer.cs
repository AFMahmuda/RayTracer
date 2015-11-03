using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;



namespace RayTracer
{

    public class RayTracer
    {
        public Bitmap TraceScene(Scene scene)
        {
            ViewPlane viewPlane = new ViewPlane(scene.Size.Width, scene.Size.Height, scene.Camera);
            Bitmap result = new Bitmap(viewPlane.PixelWidth, viewPlane.PixelHeight);
//            scene.Camera.ShowInformation();
  //          scene.ViewPlane.ShowInformation();

            int total = viewPlane.PixelWidth * viewPlane.PixelHeight;
            int count = 0;
            DateTime start = DateTime.Now;
    
            Console.WriteLine("--------------------");
            for (int row = 0; row < viewPlane.PixelHeight; row++)
            {
                for (int col = 0; col < viewPlane.PixelWidth; col++)
                {

                    Ray ray = new Ray();
                    ray.Start = scene.Camera.Position;

                    Point3 newPosition = viewPlane.GetNewLocation(col, row);
                    ray.Direction = new Vector3(ray.Start, newPosition).Normalize();

                    MyColor color = ray.Trace(scene, 0);// *.2f;
                    //if (ray.IntersectWith == null)
                    //{
                    //    int R = 135 * col / scene.ViewPlane.PixelWidth + 50;
                    //    int G = 135 * row / scene.ViewPlane.PixelHeight + 50;
                    //    int B = 135 - R + 50;
                    //    newColor = Color.FromArgb(R, G, B);
                    //}

                    //Ray ray0 = new Ray();
                    //ray0.Start = scene.Camera.Position;
                    //newPosition = viewPlane.GetNewLocation(col, row, -.5f, -.5f);
                    //ray0.Direction = new Vector3(ray.Start, newPosition).Normalize();

                    //color += ray0.Trace(scene, 0) * .2f;

                    //Ray ray1 = new Ray();
                    //ray1.Start = scene.Camera.Position;
                    //newPosition = viewPlane.GetNewLocation(col, row, -.5f, .5f);
                    //ray1.Direction = new Vector3(ray.Start, newPosition).Normalize();

                    //color += ray1.Trace(scene, 0) * .2f;

                    //Ray ray2 = new Ray();
                    //ray2.Start = scene.Camera.Position;
                    //newPosition = viewPlane.GetNewLocation(col, row, .5f, -.5f);
                    //ray2.Direction = new Vector3(ray.Start, newPosition).Normalize();

                    //color += ray2.Trace(scene, 0) * .2f;

                    //Ray ray3 = new Ray();
                    //ray3.Start = scene.Camera.Position;
                    //newPosition = viewPlane.GetNewLocation(col, row, .5f, .5f);
                    //ray3.Direction = new Vector3(ray.Start, newPosition).Normalize();

                    //color += ray3.Trace(scene, 0) * .2f;

                    result.SetPixel(col, row, color.ToColor());

                    count++;
                    if (count == total / 20)
                    {
                        Console.Write("*");
                        count = 0;
                    }
                }

            }
            Console.WriteLine();

            Console.WriteLine("finised in :"  + (DateTime.Now - start));
            return result;
        }

    }
}

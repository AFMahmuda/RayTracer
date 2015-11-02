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
            scene.ViewPlane = new ViewPlane(scene.Size.Width, scene.Size.Height, scene.Camera);
            Bitmap result = new Bitmap(scene.ViewPlane.PixelWidth, scene.ViewPlane.PixelHeight);
//            scene.Camera.ShowInformation();
  //          scene.ViewPlane.ShowInformation();

            int total = scene.ViewPlane.PixelWidth * scene.ViewPlane.PixelHeight;
            int count = 0;
            DateTime start = DateTime.Now;
    
            Console.WriteLine("--------------------");
            for (int row = 0; row < scene.ViewPlane.PixelHeight; row++)
            {
                for (int col = 0; col < scene.ViewPlane.PixelWidth; col++)
                {

                    Ray ray = new Ray();
                    ray.Start = scene.Camera.Position;

                    Point3 newPosition = scene.ViewPlane.GetNewLocation(col, row);
                    ray.Direction = new Vector3(ray.Start, newPosition);
                    ray.Direction /= ray.Direction.Magnitude;

                    Color newColor = ray.Trace(scene, 0).ToColor();
                    //if (ray.IntersectWith == null)
                    //{
                    //    int R = 135 * col / scene.ViewPlane.PixelWidth + 50;
                    //    int G = 135 * row / scene.ViewPlane.PixelHeight + 50;
                    //    int B = 135 - R + 50;
                    //    newColor = Color.FromArgb(R, G, B);

                    //}

                    result.SetPixel(col, row, newColor);

                    count++;
                    if (count == total / 20)
                    {
                        Console.Write("*");
                        count = 0;
                    }
                }

            }
            Console.WriteLine();

            Console.WriteLine("finised in :"  + (DateTime.Now- start));
            return result;
        }

    }
}

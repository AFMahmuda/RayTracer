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
            scene.ViewPlane = new ViewPlane(scene.Size.Width, scene.Size.Height);

            Bitmap result = new Bitmap(scene.ViewPlane.PixelWidth, scene.ViewPlane.PixelHeight);
            for (int row = 0; row < scene.ViewPlane.PixelWidth; row++)
            {
                for (int col = 0; col < scene.ViewPlane.PixelHeight; col++)
                {
                    Ray ray = new Ray();
                    ray.Start = scene.Camera.Position;
                    Point3 newPosition = scene.ViewPlane.GetNewLocation(scene.Camera,row, col);
                    ray.Direction = new Vector3(scene.Camera.Position, newPosition);

                    Color newColor = ray.Trace(scene,0);
                    scene.ViewPlane.SetPixel(row, col, newColor);//not needed
                    result.SetPixel(row, col, newColor);
                }
            }

            return result;
        }

    }
}

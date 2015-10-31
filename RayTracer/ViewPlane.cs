using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class ViewPlane
    {
        Camera camera;

        public Camera Camera
        {
            get { return camera; }
            set { camera = value; }
        }

        float worldWidth;

        public float WorldWidth
        {
            get { return worldWidth; }
            set { worldWidth = value; }
        }
        float worldHeight;

        public float WorldHeight
        {
            get { return worldHeight; }
            set { worldHeight = value; }
        }

        int pixelHeight;
        int pixelWidth;


        public static Point3 Position { get; set; }

        public int PixelWidth
        {
            get { return pixelWidth; }
            set { pixelWidth = value; }
        }

        public int PixelHeight
        {
            get { return pixelHeight; }
            set { pixelHeight = value; }
        }

        public ViewPlane(int width, int height, Camera camera)
        {
            this.pixelWidth = width;
            this.pixelHeight = height;


            Camera = camera;
            WorldHeight = 2 * (float)Math.Tan((camera.FieldOfView / 2f) * Math.PI / 180f);
            WorldWidth = WorldHeight * (float)((float)pixelWidth / (float)PixelHeight);

        }

        public Point3 GetUpperLeft()
        {

            Point3 center = camera.Position;
            center += camera.W.Value;
            Position = center;
            Point3 upperLeft = center + camera.U.Value * (WorldWidth / 2f) + camera.V.Value * (WorldHeight / 2f);


            return upperLeft;
        }

        public Point3 GetNewLocation(int col, int row)
        {
            Point3 upperLeft = GetUpperLeft(); 
            Point3 newLocation =
                upperLeft
                - (camera.U.Value * (col + .5f) * (WorldWidth / (float)PixelWidth))
                - (camera.V.Value * (row + .5f) * (WorldHeight / (float)PixelHeight));


            return newLocation;
        }


        public void ShowInformation()
        {
            Console.WriteLine("View Plane Information =============================");
            Console.WriteLine("Upper Left");
            GetUpperLeft().ShowInformation();
            Console.WriteLine("Center");
            Position.ShowInformation();
            Console.WriteLine("H / W pixel : " + PixelHeight + " / " + PixelWidth);
            Console.WriteLine("H / W world : " + WorldHeight + " / " + WorldWidth);
            Console.WriteLine("====================================================");
        }

    }
}

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
        float worldHeight;

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

        private Color[,] pixels;

        public ViewPlane(int width, int height, Camera camera)
        {
            this.pixelWidth = width;
            this.pixelHeight = height;
            pixels = new Color[width, height];

            Camera = camera;
            worldHeight = 2 * (float)Math.Tan(camera.FieldOfView / 2f);
            worldWidth = worldHeight * (float)(pixelWidth / PixelHeight);

        }

        public void SetPixel(int row, int col, Color color)
        {

            this.pixels[row, col] = color;
        }

        public Color GetPixel(int x, int y)
        {
            return pixels[x, y];
        }




        public Point3 GetUpperLeft()
        {

            Point3 center = camera.Position + camera.W.End;
            Position = center;
            Point3 upperLeft = center - camera.U.End * (worldWidth / 2f) - camera.V.End * (worldHeight / 2f);

            return upperLeft;
        }

        public Point3 GetNewLocation(int row, int col)
        {
            Point3 upperLeft = GetUpperLeft();
            Point3 newLocation = upperLeft + (camera.U.End * (row + .5f) * (worldWidth / (float)PixelWidth)) + (camera.V.End * (col + .5f) * (worldHeight / (float)PixelHeight));

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
            Console.WriteLine("H / W world : " + worldHeight + " / " + worldWidth);
            Console.WriteLine("====================================================");
        }

    }
}

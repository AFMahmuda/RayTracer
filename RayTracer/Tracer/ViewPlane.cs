using RayTracer.Common;
using RayTracer.Shape;
using System;

namespace RayTracer.Tracer
{
    public class ViewPlane
    {
        //TO DO: make NOT singleton

        public static ViewPlane Instance;
        float worldWidth;
        float worldHeight;
        int pixelHeight;
        int pixelWidth;
        Point3 position;
        Point3 upperLeft;
        Point3 unitRight;
        Point3 unitDown;

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


        public ViewPlane(int width, int height)
        {
            pixelWidth = width;
            pixelHeight = height;


            worldHeight = 2.0f * (float)Math.Tan((Camera.Instance.FieldOfView / 2f) * (float)Math.PI / 180.0f);
            worldWidth = worldHeight * (float)((float)pixelWidth / (float)PixelHeight);

            PreCalculate();

        }


        void PreCalculate()
        {
            upperLeft = GetUpperLeft();
            unitRight = (Camera.Instance.U.Point * (worldWidth / (float)PixelWidth) * -1);
            unitDown = (Camera.Instance.V.Point * (worldHeight / (float)PixelHeight) * -1);
        }

        Point3 GetUpperLeft()
        {

            Point3 center = Camera.Instance.Position;
            center += Camera.Instance.W.Point;
            position = center;
            Point3 upperLeft =
                center
                + Camera.Instance.U.Point * (worldWidth / 2.0f)    //U is left
                + Camera.Instance.V.Point * (worldHeight / 2.0f);  //V is up


            return upperLeft;
        }

        public Point3 GetNewLocation(int col, int row)
        {
            Point3 newLocation =
                upperLeft
                + unitRight * (col + .5f)
                + unitDown * (row + .5f);
            return newLocation;
        }

        public Point3 GetNewLocation(int col, int row, float col2, float row2)
        {

            Point3 newLocation =
                upperLeft
                + unitRight * (col + .5f + col2)
                + unitDown * (row + .5f + row2);
            return newLocation;
        }


        public void ShowInformation()
        {
            Console.WriteLine("View Plane Information =============================");
            Console.WriteLine("Upper Left");
            upperLeft.ShowInformation();
            Console.WriteLine("Center");
            position.ShowInformation();
            Console.WriteLine("H / W pixel : " + PixelHeight + " / " + PixelWidth);
            Console.WriteLine("H / W world : " + worldHeight + " / " + worldWidth);
            Console.WriteLine("====================================================");
        }

    }
}

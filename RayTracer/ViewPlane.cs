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

        Double worldWidth;
        Double worldHeight;
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


        public ViewPlane(int width, int height, Camera camera)
        {
            this.pixelWidth = width;
            this.pixelHeight = height;


            this.camera = camera;

            worldHeight = 2.0 * Math.Tan((camera.FieldOfView / 2f) * Math.PI / 180.0);
            worldWidth = worldHeight * (double)((double)pixelWidth / (double)PixelHeight);

            PreCalculate();

        }


        void PreCalculate()
        {
            upperLeft = GetUpperLeft();
            unitRight = (camera.U.Value * (worldWidth / (Double)PixelWidth) * -1);
            unitDown = (camera.V.Value * (worldHeight / (Double)PixelHeight) * -1);
        }

        Point3 GetUpperLeft()
        {

            Point3 center = camera.Position;
            center += camera.W.Value;
            position = center;
            Point3 upperLeft =
                center
                + camera.U.Value * (worldWidth / 2.0)    //U is left
                + camera.V.Value * (worldHeight / 2.0);  //V is up


            return upperLeft;
        }

        public Point3 GetNewLocation(int col, int row)
        {
            Point3 newLocation =
                upperLeft
                + unitRight * (col + .5)
                + unitDown * (row + .5);
            return newLocation;
        }

        public Point3 GetNewLocation(int col, int row, Double col2, Double row2)
        {

            Point3 newLocation =
                upperLeft
                + unitRight * (col + .5 + col2)
                + unitDown * (row + .5 + row2);
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

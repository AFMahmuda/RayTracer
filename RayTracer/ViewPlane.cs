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


        public ViewPlane(int width, int height, Camera camera)
        {
            this.pixelWidth = width;
            this.pixelHeight = height;


            this.camera = camera;

            worldHeight = 2 * (float)Math.Tan((camera.FieldOfView / 2f) * Math.PI / 180f);
            worldWidth = worldHeight * (float)((float)pixelWidth / (float)PixelHeight);

            PreCalculate();

        }


        void PreCalculate()
        {
            upperLeft = GetUpperLeft();
            unitRight =(camera.U.Value * (worldWidth / (float)PixelWidth) * -1);
            unitDown = (camera.V.Value * (worldHeight / (float)PixelHeight) * -1);
        }

        Point3 GetUpperLeft()
        {

            Point3 center = camera.Position;
            center += camera.W.Value;
            position = center;
            Point3 upperLeft =
                center
                + camera.U.Value * (worldWidth / 2f)    //U is left
                + camera.V.Value * (worldHeight / 2f);  //V is up


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

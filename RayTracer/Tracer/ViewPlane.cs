using RayTracer.Common;
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
            worldWidth = worldHeight * (pixelWidth / (float)PixelHeight);

            PreCalculate();

        }


        void PreCalculate()
        {
            upperLeft = GetUpperLeft();
            unitRight = (Camera.Instance.U * (worldWidth / (float)PixelWidth) * -1);
            unitDown = (Camera.Instance.V * (worldHeight / (float)PixelHeight) * -1);
        }

        Point3 GetUpperLeft()
        {

            Point3 center = Camera.Instance.Position;
            center += Camera.Instance.W;
            position = center;
            Point3 upperLeft =
                center
                + Camera.Instance.U * (worldWidth / 2.0f)    //U is left
                + Camera.Instance.V * (worldHeight / 2.0f);  //V is up


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

        public Point3 GetNewLocation(int col, int row, float colOffset, float rowOffset)
        {
            Point3 newLocation =
                upperLeft
                + unitRight * (col + .5f + colOffset)
                + unitDown * (row + .5f + rowOffset);
            return newLocation;
        }
    }
}

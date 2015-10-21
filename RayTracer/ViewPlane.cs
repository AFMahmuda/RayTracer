using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class ViewPlane
    {

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

        public ViewPlane(int width, int height)
        {
            this.pixelWidth = width;
            this.pixelHeight = height;
            pixels = new Color[width, height];
        }

        public void SetPixel(int row, int col, Color color)
        {

            this.pixels[row, col] = color;
        }

        public Color GetPixel(int x, int y)
        {
            return pixels[x, y];
        }




        public Point3 GetLeftButtom(Vector3 W, Vector3 U, Vector3 V)
        {

            Point3 result;
            Vector3 Center = new Vector3(Position) - W;
            Vector3 BottomLeft = Center - U * PixelWidth / 2 - V * PixelHeight / 2;
            result = BottomLeft.End;

            return result;
        }

        public Point3 GetNewLocation(Camera camera, int row, int col)
        {

            Point3 result;
            Vector3 Center = new Vector3(Position) - camera.W;
            Vector3 BottomLeft = Center - camera.U * row * PixelWidth / 2 - camera.V * col * PixelHeight / 2;
            result = BottomLeft.End;

            return result;
        }

    }
}

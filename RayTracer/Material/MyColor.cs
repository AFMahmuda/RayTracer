using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RayTracer.Material
{
    [Serializable]
    public class MyColor
    {
        private float r;
        private float g;
        private float b;

        public float R
        {
            get { return r; }
            set { r = (value < 0) ? 0 : ((value > 1) ? 1 : value); }
        }
        public float G
        {
            get { return g; }
            set { g = (value < 0) ? 0 : ((value > 1) ? 1 : value); }
        }
        public float B
        {
            get { return b; }
            set { b = (value < 0) ? 0 : ((value > 1) ? 1 : value); }
        }

        public MyColor(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }

        public MyColor(float[] param)
            : this(param[0], param[1], param[2])
        { }
        public MyColor()
            : this(0f, 0f, 0f)
        { }

        public Color ToColor()
        {
            int r = (int)(R * 255);
            int g = (int)(G * 255);
            int b = (int)(B * 255);
            return Color.FromArgb(r, g, b);
        }

        public static MyColor operator +(MyColor color1, MyColor color2)
        {
            return new MyColor(
                color1.R + color2.R,
                color1.G + color2.G,
                color1.B + color2.B
                );
        }

        public static MyColor operator -(MyColor color1, MyColor color2)
        {
            return new MyColor(
                color1.R - color2.R,
                color1.G - color2.G,
                color1.B - color2.B
                );
        }

        public static MyColor operator *(MyColor color1, MyColor color2)
        {
            return new MyColor(
                color1.R * color2.R,
                color1.G * color2.G,
                color1.B * color2.B
                );
        }

        public static MyColor operator *(MyColor color, float value)
        {
            return new MyColor(
                color.R * value,
                color.G * value,
                color.B * value
                );
        }

        public static MyColor operator *(float value, MyColor color)
        {
            return color * value;
        }

        public MyColor Pow(float value)
        {
            R = (float)Math.Pow(R, value);
            G = (float)Math.Pow(G, value);
            B = (float)Math.Pow(B, value);

            return this;
        }

    }


}

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
        private Double r;
        private Double g;
        private Double b;

        public Double R
        {
            get { return r; }
            set
            {
                r = (value < 0) ? 0 : ((value > 1) ? 1 : value);
            }
        }
        public Double G
        {
            get { return g; }
            set
            {
                g = (value < 0) ? 0 : ((value > 1) ? 1 : value);
            }
        }
        public Double B
        {
            get { return b; }
            set
            {
                b = (value < 0) ? 0 : ((value > 1) ? 1 : value);
            }
        }

        public MyColor(Double r, Double g, Double b)
        {
            R = r;
            G = g;
            B = b;
        }

        public MyColor(Double[] param)
            : this(param[0], param[1], param[2])
        { }
        public MyColor()
            : this(0f, 0f, 0f)
        { }


        public MyColor Clone()
        {
            return (MyColor)this.MemberwiseClone();
        }

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

        public static MyColor operator *(MyColor color, Double value)
        {
            return new MyColor(
                color.R * value,
                color.G * value,
                color.B * value
                );
        }

        public static MyColor operator *(Double value, MyColor color)
        {
            return color * value;
        }

        public MyColor Pow(Double value)
        {
            R = Math.Pow(R, value);
            G = Math.Pow(G, value);
            B = Math.Pow(B, value);

            return this;
        }

    }


}

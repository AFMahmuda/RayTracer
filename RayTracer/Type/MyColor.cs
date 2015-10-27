using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class MyColor
    {
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }

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


        public MyColor Clone()
        {
            return (MyColor)this.MemberwiseClone();
        }

        public Color ToColor()
        {
            int r = (int)R * 255;
            int g = (int)G * 255;
            int b = (int)B * 255;
            return Color.FromArgb(r, g, b);
        }

    }


}

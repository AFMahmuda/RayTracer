using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Light
    {
        public Color Ambient
        { get; set; }

        public Color Color
        { get; set; }

        public struct Attenuation
        {
            float constan;

            public float Constan
            {
                get { return constan; }
                set { constan = value; }
            }
            float linear;

            public float Linear
            {
                get { return linear; }
                set { linear = value; }
            }
            float quadratic;

            public float Quadratic
            {
                get { return quadratic; }
                set { quadratic = value; }
            }
        }
    }

    
}

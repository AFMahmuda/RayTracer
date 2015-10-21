using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Material
    {
        public Color specular
        { get; set; }

        public Color diffuse
        { get; set; }

        public Color emission
        { get; set; }

        public float shininess
        { get; set; }
    }
}

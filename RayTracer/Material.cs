using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Material
    {

        public Material()
        {
            Specular = Color.Gray;
            Diffuse = Color.Gray;
            Emission = Color.Gray;
            Shininess = .5f;
        }

        public Color Specular
        { get; set; }

        public Color Diffuse
        { get; set; }

        public Color Emission
        { get; set; }

        public float Shininess
        { get; set; }

        public Material Clone()
        {
            return (Material)this.MemberwiseClone();
        }
    }
}

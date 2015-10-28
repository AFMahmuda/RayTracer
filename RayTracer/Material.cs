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
            Specular = new MyColor(.4f,.4f,.4f);
            Diffuse = new MyColor(.4f, .4f, .4f);
            Emission = new MyColor(.4f, .4f, .4f);
            Shininess = .5f;
        }

        public MyColor Specular
        { get; set; }

        public MyColor Diffuse
        { get; set; }

        public MyColor Emission
        { get; set; }

        public float Shininess
        { get; set; }

        public Material Clone()
        {
            return (Material)this.MemberwiseClone();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RayTracer
{
    [Serializable]
    public class Material
    {

        public Material()
        {
            Specular = new MyColor();
            Diffuse = new MyColor();
            Emission = new MyColor();
            Shininess = 0f;
        }

        public MyColor Specular
        { get; set; }

        public MyColor Diffuse
        { get; set; }

        public MyColor Emission
        { get; set; }


        Double shininess;
        public Double Shininess
        {
            get
            {
                return shininess;
            }
            set
            {
                shininess = (value < 0) ? 0 : ((value > 128) ? 128 : value);
            }
        }

        public Material Clone()
        {
            return (Material)this.MemberwiseClone();
        }
    }
}

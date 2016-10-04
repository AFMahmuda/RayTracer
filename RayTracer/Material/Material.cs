using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RayTracer.Material
{
    [Serializable]
    public class Mat
    {

        public Mat()
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


        float shininess = 0;
        public float Shininess
        {
            get
            { return shininess; }
            set
            { shininess = (value < 0) ? 0 : ((value > 128) ? 128 : value); }
        }

        float refractValue = 0;
        public float RefractValue
        {
            get
            { return refractValue; }
            set
            { refractValue = (value < 0) ? 0 : ((value > 1) ? 1 : value); }
        }

        float refractIndex = 1;
        public float RefractIndex
        {
            get
            { return refractIndex; }
            set
            { refractIndex = (value < 0) ? 0 : value; }
        }

        public Mat Clone()
        {
            return (Mat)this.MemberwiseClone();
        }
    }
}

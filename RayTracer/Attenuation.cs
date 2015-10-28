using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    class Attenuation
    {
        public Attenuation()
        {
            Constant = 1f;
            Linear = 0f;
            Quadratic = 0f;
        }

        public Attenuation(float[] param)
        {
            Constant = param[0];
            Linear = param[1];
            Quadratic = param[2];

        }
        float constant;

        public float Constant
        {
            get { return constant; }
            set { constant = value; }
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

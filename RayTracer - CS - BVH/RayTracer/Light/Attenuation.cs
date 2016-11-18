using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Lighting
{
    [Serializable]
    public class Attenuation
    {
        public Attenuation()
        {
            constant = 1f;
            linear = 0f;
            quadratic = 0f;
        }

        public Attenuation(float[] param)
        {
            constant = param[0];
            linear = param[1];
            quadratic = param[2];

        }
        float constant;

        public float Constant
        {
            get { return constant; }
        }
        float linear;

        public float Linear
        {
            get { return linear; }
        }
        float quadratic;

        public float Quadratic
        {
            get { return quadratic; }
        }

    }
}

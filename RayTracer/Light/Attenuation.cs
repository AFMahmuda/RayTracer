using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
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

        public Attenuation(Double[] param)
        {
            constant = param[0];
            linear = param[1];
            quadratic = param[2];

        }
        Double constant;

        public Double Constant
        {
            get { return constant; }
        }
        Double linear;

        public Double Linear
        {
            get { return linear; }
        }
        Double quadratic;

        public Double Quadratic
        {
            get { return quadratic; }
        }

    }
}

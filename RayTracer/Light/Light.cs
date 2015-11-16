using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RayTracer
{
    [Serializable]
    public abstract class Light
    {

        public MyColor Color
        { get; set; }

        public abstract Vector3 GetPointToLight(Point3 point);
        public abstract bool IsEffective(Point3 point, Geometry geometry, List<Geometry> geometries);
        public abstract Double GetAttenuationValue(Point3 point, Attenuation attenuation);
        
    }


}

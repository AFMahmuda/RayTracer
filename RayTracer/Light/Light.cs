using RayTracer.Common;
using RayTracer.Material;
using RayTracer.Shape;
using System;
using System.Collections.Generic;


namespace RayTracer.Lighting
{
    [Serializable]
    public abstract class Light
    {

        public MyColor Color
        { get; set; }

        public abstract Vector3 GetPointToLight(Point3 point);
        public abstract bool IsEffective(Point3 point, Geometry geometry, List<Geometry> geometries);
        public abstract Double GetAttValue(Point3 point, Attenuation attenuation);

    }


}

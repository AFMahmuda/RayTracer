using RayTracer.Common;
using RayTracer.Shape;
using RayTracer.Tracer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer.Lighting
{
    [Serializable]
    public class PointLight : Light
    {

        public PointLight(Point3 from, MyColor color)
        {
            Position = from;
            Color = color;

        }
        public PointLight(Double[] param)
            : this(new Point3(param[0], param[1], param[2]), new MyColor(param[3], param[4], param[5]))
        {
        }

        public Point3 Position
        { get; set; }

        public override Vector3 GetPointToLight(Point3 point)
        {
            return new Vector3(point, Position);
        }

        public override bool IsEffective(Point3 point, Geometry geometry, List<Geometry> geometries)
        {
            if (GetPointToLight(point) * geometry.GetNormal(point) < 0)
                return false;

            Ray shadowRay = new Ray(point, GetPointToLight(point));
            foreach (var item in geometries)
            {
                if (item.Equals(geometry))
                    continue;

                Point3 pos = Utils.DeepClone(shadowRay.Start);
                Vector3 dir = Utils.DeepClone(shadowRay.Direction);

                shadowRay.TransformInv(item.Trans);

                if (item.IsIntersecting(shadowRay))
                    if (shadowRay.IntersectDistance < GetPointToLight(point).Magnitude)
                        return false;
                
                shadowRay.Start = pos;
                shadowRay.Direction = dir;
            }

            return true;
        }

        public override Double GetAttValue(Point3 point, Attenuation attenuation)
        {
            if (!attenuation.Equals(new Attenuation()))
                return 1f;

            Double d = GetPointToLight(point).Magnitude;
            return 1f /
                (attenuation.Constant +
                attenuation.Linear * d +
                attenuation.Quadratic * Math.Pow(d, 2));

        }

    }
}

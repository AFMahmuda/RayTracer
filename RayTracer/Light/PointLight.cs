using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    [Serializable]
    public class PointLight : Light
    {

        public PointLight(Point3 from, MyColor color)
        {
            Position = from;
            Color = color;

        }
        public PointLight(float[] param)
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
            Ray ray = new Ray(point, GetPointToLight(point));
            foreach (var item in geometries)
            {
                if (item.Equals(geometry))
                    continue;

                Point3 pos = Utils.DeepClone(ray.Start);
                Vector3 dir = Utils.DeepClone(ray.Direction);
                ray.TransformInv(item.Transform);

                if (item.IsIntersecting(ray))
                    if (ray.IntersectDistance < GetPointToLight(point).Magnitude)
                        return false;
                
                ray.Start = pos;
                ray.Direction = dir;
                //ray.Transform(item.Transform);
            }

            return true;
        }

        public override float GetAttenuationValue(Point3 point, Attenuation attenuation)
        {
            if (!attenuation.Equals(new Attenuation()))
                return 1f;

            float d = GetPointToLight(point).Magnitude;
            return 1f /
                (attenuation.Constant +
                attenuation.Linear * d +
                attenuation.Quadratic * (float)Math.Pow(d, 2));

        }

    }
}

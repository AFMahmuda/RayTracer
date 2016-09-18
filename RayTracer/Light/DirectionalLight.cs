using RayTracer.Common;
using RayTracer.Material;
using RayTracer.Shape;
using RayTracer.Tracer;
using System;
using System.Collections.Generic;

namespace RayTracer.Lighting
{
    [Serializable]
    public class DirectionalLight : Light
    {


        public DirectionalLight(Vector3 direction, MyColor color)
        {
            Direction = direction;
            Color = color;

        }
        public DirectionalLight(Double[] param)
            : this(new Vector3(param[0], param[1], param[2]), new MyColor(param[3], param[4], param[5]))
        {
        }

        public Vector3 Direction
        {
            get;
            set;
        }

        public override Vector3 GetPointToLight(Point3 point)
        {
            return new Vector3(Direction.Point * -1);
        }

        public override bool IsEffective(Point3 point, Geometry geometry, List<Geometry> geometries)
        {
            //not needed. lights effective to both sides
            //if (GetPointToLight(point) * geometry.GetNormal(point) < 0)
            //    return false;

            Ray ray = new Ray(point, GetPointToLight(point));
            foreach (var item in geometries)
            {
                ray.TransformInv(item.Trans);
                if (item.IsIntersecting(ray))
                    return false;
                ray.Transform(item.Trans);
            }

            return true;

        }

        public override Double GetAttValue(Point3 point, Attenuation attenuation)
        {
            return 1;
        }



    }
}

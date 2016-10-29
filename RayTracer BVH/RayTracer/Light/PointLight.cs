using RayTracer.Common;
using RayTracer.Material;
using RayTracer.Tracer;
using System;
using RayTracer.BVH;
using RayTracer.Transformation;

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
        public PointLight(float[] param)
            : this(new Point3(param[0], param[1], param[2]), new MyColor(param[3], param[4], param[5]))
        {
        }

        public Point3 Position
        { get; set; }

        public override Vec3 GetPointToLight(Point3 point)
        {
            return new Vec3(point, Position);
        }

        public override bool IsEffective(Point3 point, Container bvh)
        {
            Vec3 pointToLight = GetPointToLight(point);

            Ray shadowRay = new Ray(point, pointToLight.Normalize());

            if (bvh.IsIntersecting(shadowRay))
            {
                if (bvh.Geo != null)
                {
                    if (bvh.Geo.Trans != new Translation())
                        shadowRay.TransformInv(bvh.Geo.Trans);
                    if (bvh.Geo.IsIntersecting(shadowRay))
                        if (shadowRay.IntersectDistance < pointToLight.Magnitude)
                            return false;
                }
                else
                {
                    for (int i = 0; i < bvh.Childs.Length; i++)
                        if (!IsEffective(point, bvh.Childs[i])) return false;
                }
            }

            return true;
        }

        public override float GetAttValue(Point3 point, Attenuation attenuation)
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

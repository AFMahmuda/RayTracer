using RayTracer.Common;
using RayTracer.Material;
using RayTracer.Shape;
using RayTracer.Tracer;
using System;
using System.Collections.Generic;
using RayTracer.BVH;

namespace RayTracer.Lighting
{
    [Serializable]
    public class DirectionalLight : Light
    {


        public DirectionalLight(Vector3 direction, MyColor color)
        {
            Direction = direction.Normalize();
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

        public override bool IsEffective(Point3 point, Geometry geometry, Container bvh)
        {
            Ray shadowRay = new Ray(point, (Direction*-1));
            if ((Direction*-1) * geometry.GetNormal(point) < 0)
                return false;
            if (bvh.IsIntersecting(shadowRay))
            {
                if (bvh.Geo != null)
                {
                    shadowRay.TransformInv(bvh.Geo.Trans);
                    if (bvh.Geo.IsIntersecting(shadowRay))
                            return false;
                }
                else
                {
                    foreach (Container bin in bvh.Childs)
                        if (!IsEffective(point, geometry, bin)) return false;
                }
            }

            return true;
        }
        

        public override double GetAttValue(Point3 point, Attenuation attenuation)
        {
            return 1;
        }


    }
}

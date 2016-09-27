﻿using RayTracer.Common;
using RayTracer.Material;
using RayTracer.Shape;
using RayTracer.Tracer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RayTracer.BVH;

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
        
        public override bool IsEffective(Point3 point, Geometry geometry, Container bvh)
        {
            Vector3 pointToLight = GetPointToLight(point);
            if (pointToLight * geometry.GetNormal(point) < 0)
                return false;

            Ray shadowRay = new Ray(point, pointToLight.Normalize());

            if (bvh.IsIntersecting(shadowRay))
            {
                if (bvh.Geo != null)
                {
                    shadowRay.TransformInv(bvh.Geo.Trans);
                    if (bvh.Geo.IsIntersecting(shadowRay))
                        if (shadowRay.IntersectDistance < pointToLight.Magnitude)
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

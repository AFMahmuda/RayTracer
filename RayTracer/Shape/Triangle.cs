﻿using RayTracer.Common;
using RayTracer.Tracer;
using RayTracer.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer.Shape
{
    [Serializable]
    public class Triangle : Geometry
    {
        public Point3 a;
        public Point3 b;
        public Point3 c;


        //precalculated vals
        private Vector3 localNorm;

        private Vector3 ab;
        private Vector3 ac;

        private Double dot_ab_ab;
        private Double dot_ab_ac;
        private Double dot_ac_ac;
        private Double dot_ab_ap;
        private Double dot_ac_ap;

        private Double invDenom;

        public Triangle(Point3 a, Point3 b, Point3 c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            Trans = new Translation();
            hasMorton = false;
            PreCalculate();
        }

        void PreCalculate()
        {

            //for IsInsideTriangle
            ab = new Vector3(a, b);
            ac = new Vector3(a, c);

            //for IsIntersect
            localNorm = Vector3.Cross(ac, ab).Normalize();

            dot_ab_ab = ab * ab;
            dot_ab_ac = ab * ac;
            dot_ac_ac = ac * ac;

            invDenom = 1.0 / (dot_ab_ab * dot_ac_ac - dot_ab_ac * dot_ab_ac);
        }

        public override bool IsIntersecting(Ray ray)
        {
            //parallel -> return false
            if (ray.Direction * localNorm == 0)
                return false;
            /*
            relative to ray direction
            */
            Double distanceToPlane = (
                 (new Vector3(a) * localNorm) -
                 (new Vector3(ray.Start) * localNorm))
                / (ray.Direction * localNorm);
            /*
            dist < 0 = behind cam
            */
            if (distanceToPlane > 0)
                if (ray.IsSmallerThanCurrent(distanceToPlane, Trans))
                    if (IsInsideTriangle(ray.Start + (ray.Direction * distanceToPlane).Point))
                    {
                        ray.IntersectDistance = MyMatrix.Mult44x41(Trans.Matrix, ray.Direction * distanceToPlane, 0).Magnitude;
                        return true;
                    }
            return false;

        }


        bool IsInsideTriangle(Point3 point)
        {
            Vector3 ap = new Vector3(point - a);

            dot_ab_ap = ab * ap;
            dot_ac_ap = ac * ap;

            Double u = (dot_ac_ac * dot_ab_ap - dot_ab_ac * dot_ac_ap) * invDenom;
            Double v = (dot_ab_ab * dot_ac_ap - dot_ab_ac * dot_ab_ap) * invDenom;

            return (u >= 0) && (v >= 0) && (u + v <= 1);
        }


        public override Vector3 GetNormal(Point3 point)
        {
            return MyMatrix.Mult44x41(Trans.Matrix.Inverse, localNorm, 0).Normalize();
        }

        public override void UpdatePos()
        {
            Vector3 temp = new Vector3(a + b + c) * (.33f);
            pos = MyMatrix.Mult44x41(Trans.Matrix, temp, 1).Point;
            pos.X /= (10) + .5;
            pos.Y /= (10) + .5;
            pos.Z /= (10) + .5;
        }
    }
}

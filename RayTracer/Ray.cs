﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Ray
    {
        public Ray()
        {
            IntersectDistance = double.MaxValue;
            IntersectWith = null;
        }

        public Ray(Point3 start, Vector3 direction)
            : this()
        {
            Start = start;
            Direction = direction;
        }

        public Point3 Start
        { get; set; }

        public Vector3 Direction
        { get; set; }

        public Double IntersectDistance
        { get; set; }

        public Geometry IntersectWith
        { get; set; }


        Point3 HitPoint
        {
            get { return Start + (Direction * (IntersectDistance * (.999f))).Point; }
        }

        public MyColor Trace(Scene scene, int bounce = 0)
        {
            if (bounce > scene.MaxDepth)
                return new MyColor();

            try
            {
                foreach (var geometry in scene.Geometries)
                {
                    TransformInv(geometry.Transform);
                    if (geometry.IsIntersecting(this))
                        IntersectWith = geometry;
                    Transform(geometry.Transform);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("from ray.trace " + e.InnerException + e.Message);
                throw;
            }

            if (IntersectWith != null)
            {
                List<Light> effectiveLight = PopulateEffectiveLight(scene.Lights, scene.Geometries);
                MyColor color = CalculateColor(effectiveLight, scene.Attenuation);

                Vector3 reflection = Direction - (IntersectWith.GetNormal(HitPoint) * 2 * (Direction * IntersectWith.GetNormal(HitPoint)));
                Ray reflectedRay = new Ray(HitPoint, reflection);
                double reflectability = .35;
                return color + (reflectability * reflectedRay.Trace(scene, bounce + 1));
            }

            else return new MyColor();
        }

        MyColor CalculateColor(List<Light> effectiveLights, Attenuation attenuation)
        {

            MyColor result = IntersectWith.Ambient + IntersectWith.Material.Emission;
            Vector3 normal = IntersectWith.GetNormal(HitPoint);


            foreach (var light in effectiveLights)
            {
                Vector3 pointToLight = light.GetPointToLight(HitPoint);
                Vector3 halfAngleToLight = (Direction * -1f + pointToLight).Normalize();

                Material material = IntersectWith.Material;

                double attenuationValue = light.GetAttenuationValue(HitPoint, attenuation);

                result +=
                    attenuationValue * light.Color *
                    (
                        material.Diffuse * (pointToLight.Normalize() * normal) +
                        (material.Specular * Math.Pow(halfAngleToLight * normal, material.Shininess))
                    );
            }
            return result;

        }

        List<Light> PopulateEffectiveLight(List<Light> allLights, List<Geometry> geometries)
        {
            List<Light> result = new List<Light>();
            foreach (var light in allLights)
            {
                if (light.IsEffective(HitPoint, IntersectWith, geometries))
                    result.Add(light);
            }

            return result;
        }

        public void Transform(Transform transform)
        {
            Start = MyMatrix.Mult44x41(transform.Matrix, new Vector3(Start), 1).Point;
            Direction = MyMatrix.Mult44x41(transform.Matrix, Direction, 0).Normalize();
        }

        public void TransformInv(Transform transform)
        {
            Start = MyMatrix.Mult44x41(transform.Matrix.Inverse, new Vector3(Start), 1).Point;
            Direction = MyMatrix.Mult44x41(transform.Matrix.Inverse, Direction, 0).Normalize();
        }
        public bool IsSmallerThanCurrent(Double distance, Transform trans)
        {
            double newMagnitude = MyMatrix.Mult44x41(trans.Matrix, Direction * distance, 0).Magnitude;
            return (newMagnitude < IntersectDistance) ? true : false;
        }

        void ShowInformation()
        {
            Console.WriteLine("Ray Information======================================");
            Console.Write("Start : ");
            Start.ShowInformation();
            Console.WriteLine("Direction");
            Direction.ShowInformation();
            Console.WriteLine("=====================================================");
        }


    }
}

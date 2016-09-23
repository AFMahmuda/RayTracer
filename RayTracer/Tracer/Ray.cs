using RayTracer.Common;
using RayTracer.Shape;
using RayTracer.Lighting;
using RayTracer.Material;
using RayTracer.Transformation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using RayTracer.BVH;

namespace RayTracer.Tracer
{
    public class Ray
    {


        public Ray()
        {
            IntersectDistance = double.MaxValue;
            IntersectWith = null;
            Type = TYPE.RAY;
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
        public enum TYPE
        {
            RAY,
            REFLECTION,
            REFRACTION
        }

        public TYPE Type { get; set; }

        Point3 HitPointMinus
        {
            get { return Start + (Direction * (IntersectDistance * (.999f))).Point; }
        }

        Point3 HitPointPlus
        {
            get { return Start + (Direction * (IntersectDistance * (1.001f))).Point; }
        }
        Point3 RealHitPoint
        {
            get { return Start + (Direction * (IntersectDistance)).Point; }
        }

        public void Trace(Scene scene, Container Bvh)
        {

            //test
            //            return new MyColor();

            try
            {
                if (Bvh.IsIntersecting(this))
                {
                    if (Bvh.Geo != null)
                    {
                        Point3 tempStart = Utils.DeepClone(Start);
                        Vector3 tempDir = Utils.DeepClone(Direction);

                        //transform ray according to each shapes transformation
                        TransformInv(Bvh.Geo.Trans);

                        if (Bvh.Geo.IsIntersecting(this))
                            IntersectWith = Bvh.Geo;

                        //assign original value for start and direction by memory
                        Start = tempStart;
                        Direction = tempDir;
                        //Transform(geometry.Trans);
                    }
                    else
                    {
                        foreach (Container bin in Bvh.Childs)
                            Trace(scene, bin);
                    }
                }

                ////save original value
                //Point3 tempStart = Utils.DeepClone(Start);
                //Vector3 tempDir = Utils.DeepClone(Direction);
                //foreach (var geometry in scene.Geometries)
                //{
                //    //transform ray according to each shapes transformation
                //    TransformInv(geometry.Trans);

                //    if (geometry.IsIntersecting(this))
                //        IntersectWith = geometry;

                //    //assign original value for start and direction by memory
                //    Start = tempStart;
                //    Direction = tempDir;
                //    //Transform(geometry.Trans);
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine("from ray.trace " + e.InnerException + e.Message);
                throw;
            }

        }


        public MyColor GetColor(Scene scene, int bounce)
        {
            if (bounce <= 0 || IntersectWith == null)
                return new MyColor();
            else
            {
                List<Light> effectiveLights = PopulateEffectiveLight(scene.Lights, scene.Geometries);
                MyColor color = CalcColor(effectiveLights, scene.Attenuation);

                return color +
                    CalcReflection(scene, bounce - 1);
                    //CalcRefraction(scene, bounce - 1) ;
            }
        }
        MyColor CalcReflection(Scene scene, int bounce)
        {
            if (Type != TYPE.REFRACTION)
            {
                Vector3 rreflectDir = Direction - (IntersectWith.GetNormal(RealHitPoint) * 2.0 * (Direction * IntersectWith.GetNormal(HitPointMinus)));
                Ray reflectRay = new Ray(HitPointMinus, rreflectDir);
                //reflectRay.Type = TYPE.REFLECTION;
                reflectRay.Trace(scene, scene.Bvh);
                return IntersectWith.Material.Specular * reflectRay.GetColor(scene,bounce);
            }
            else return new MyColor();
        }

        //        MyColor CalcRefraction(Scene scene, int bounce)
        //        {
        //            if (IntersectWith.GetType() == typeof(Sphere))
        //            {
        //                Vector3 refDir = Direction;
        //                Ray refRay = new Ray(HitPointPlus, refDir);
        ////              refRay.Type = TYPE.REFRACTION;
        //                return (refRay.Trace(scene, bounce));
        //            }
        //            else return new MyColor();
        //        }



        MyColor CalcColor(List<Light> effectiveLights, Attenuation attenuation)
        {

            MyColor result = IntersectWith.Ambient + IntersectWith.Material.Emission;
            Vector3 normal = IntersectWith.GetNormal(HitPointMinus);

            foreach (var light in effectiveLights)
            {
                Vector3 pointToLight = light.GetPointToLight(HitPointMinus);
                Vector3 halfAngleToLight = (Direction * -1f + pointToLight).Normalize();

                Mat material = IntersectWith.Material;

                double attenuationValue = light.GetAttValue(HitPointMinus, attenuation);

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
                if (light.IsEffective(HitPointMinus, IntersectWith, geometries))
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
    }
}

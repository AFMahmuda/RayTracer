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
            IntersectDistance = float.MaxValue;
            IntersectWith = null;
            Type = TYPE.RAY;
        }

        public Ray(Point3 start, Vec3 direction)
            : this()
        {
            Start = start;
            Direction = direction;
        }

        public Point3 Start
        { get; set; }

        public Vec3 Direction
        { get; set; }

        public float IntersectDistance
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
            try
            {
                if (Bvh.IsIntersecting(this))
                {
                    if (Bvh.Geo != null)
                    {
                        Point3 tempStart = Utils.DeepClone(Start);
                        Vec3 tempDir = Utils.DeepClone(Direction);

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
            }
            catch (Exception e)
            {
                Console.WriteLine("from ray.trace " + e.InnerException + e.Message);
                throw;
            }

        }

        MyColor CalcColor(List<Light> effectiveLights, Attenuation attenuation)
        {

            MyColor result = IntersectWith.Ambient + IntersectWith.Material.Emission;
            Vec3 normal = IntersectWith.GetNormal(HitPointMinus);

            foreach (var light in effectiveLights)
            {
                Vec3 pointToLight = light.GetPointToLight(HitPointMinus);
                Vec3 halfAngleToLight = ((Direction * -1.0f) + pointToLight).Normalize();

                Mat material = IntersectWith.Material;

                float attenuationValue = light.GetAttValue(HitPointMinus, attenuation);

                result +=
                    attenuationValue * light.Color *
                    (
                        material.Diffuse * (pointToLight.Normalize() * normal) +
                        (material.Specular * (float)Math.Pow(halfAngleToLight * normal, material.Shininess))
                    );
            }
            return result;

        }
        public MyColor GetColor(Scene scene, int bounce)
        {
            if (bounce <= 0 || IntersectWith == null)
                return new MyColor();
            else
            {
                List<Light> effectiveLights = PopulateEffectiveLight(scene.Lights, scene.Bvh);
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
//                Vec3 rreflectDir = Direction - (IntersectWith.GetNormal(RealHitPoint) * 2.0f * (Direction * IntersectWith.GetNormal(RealHitPoint)));
                Vec3 rreflectDir = Direction + (IntersectWith.GetNormal(RealHitPoint) * 2.0f * -(IntersectWith.GetNormal(RealHitPoint)* Direction ));
                Ray reflectRay = new Ray(HitPointMinus, rreflectDir);
                reflectRay.Type = TYPE.REFLECTION;
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



       

        List<Light> PopulateEffectiveLight(List<Light> allLights, Container bvh)
        {
            List<Light> result = new List<Light>();
            foreach (var light in allLights)
            {
                if (light.IsEffective(HitPointMinus, IntersectWith, bvh))
                    result.Add(light);
            }

            return result;
        }

        public void Transform(Transform transform)
        {
            Start = Common.Mattrix.Mul44x41(transform.Matrix, new Vec3(Start), 1).Point;
            Direction = Common.Mattrix.Mul44x41(transform.Matrix, Direction, 0).Normalize();
        }

        public void TransformInv(Transform transform)
        {
            Start = Common.Mattrix.Mul44x41(transform.Matrix.Inverse, new Vec3(Start), 1).Point;
            Direction = Common.Mattrix.Mul44x41(transform.Matrix.Inverse, Direction, 0).Normalize();
        }
        public bool IsSmallerThanCurrent(float distance, Transform trans)
        {
            float newMagnitude = Common.Mattrix.Mul44x41(trans.Matrix, Direction * distance, 0).Magnitude;
            return (newMagnitude < IntersectDistance) ? true : false;
        }
    }
}

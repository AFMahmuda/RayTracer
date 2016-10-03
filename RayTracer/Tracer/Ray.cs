using RayTracer.Common;
using RayTracer.Shape;
using RayTracer.Lighting;
using RayTracer.Material;
using RayTracer.Transformation;
using System;
using System.Collections.Generic;
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

                        //assign original value for start and direction by memory equivalent to Transform(geometry.Trans);
                        Start = tempStart;
                        Direction = tempDir;

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
                       CalcReflection(scene, bounce - 1) +
                       CalcRefraction(scene, bounce - 1);
            }
        }
        MyColor CalcReflection(Scene scene, int bounce)
        {
            if (Type != TYPE.REFRACTION)
            {
                Vec3 newDir = Direction + (IntersectWith.GetNormal(RealHitPoint) * 2.0f * -(IntersectWith.GetNormal(RealHitPoint) * Direction));
                Ray reflectRay = new Ray(HitPointMinus, newDir);
                reflectRay.Type = TYPE.REFLECTION;
                reflectRay.Trace(scene, scene.Bvh);
                return IntersectWith.Material.Specular * reflectRay.GetColor(scene, bounce);
            }
            else return new MyColor();
        }

        MyColor CalcRefraction(Scene scene, int bounce)
        {
            if (IntersectWith.GetType() == typeof(Sphere))
            {
                float cosI = IntersectWith.GetNormal(RealHitPoint) * Direction;
                Vec3 normal;
                float n1, n2, n;

                if (cosI > 0)
                {
                    n1 = 1.25F;
                    n2 = 1;
                    normal = IntersectWith.GetNormal(RealHitPoint) * -1;
                }
                else
                {
                    n1 = 1;
                    n2 = 1.25F;
                    normal = IntersectWith.GetNormal(RealHitPoint);
                    cosI = -cosI;
                }
                n = n1 / n2;
                float sinT2 = n * n * (1f - cosI * cosI);
                float cosT = (float)Math.Sqrt(1f - sinT2);



                float rn = (n1 * cosI - n2 * cosT) / (n1 * cosI + n2 * cosT);
                float rt = (n2 * cosI - n1 * cosT) / (n2 * cosI + n2 * cosT);
                rn *= rn;
                rt *= rt;
                float refl = (rn + rt) * .5f;
                float trans = 1f - refl;

                if (cosT * cosT < 0)
                {
                    return new MyColor();
                }





                Vec3 newDir = Direction * n + normal * (n * cosI - cosT);
                Ray refractRay = new Ray(HitPointPlus, newDir);
                refractRay.Type = TYPE.REFRACTION;
                refractRay.Trace(scene, scene.Bvh);
                return (refractRay.GetColor(scene, bounce));
            }
            else return new MyColor();
        }

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
            Start = Mattrix.Mul44x41(transform.Matrix, new Vec3(Start), 1).Point;
            Direction = Mattrix.Mul44x41(transform.Matrix, Direction, 0).Normalize();
        }

        public void TransformInv(Transform transform)
        {
            Start = Mattrix.Mul44x41(transform.Matrix.Inverse, new Vec3(Start), 1).Point;
            Direction = Mattrix.Mul44x41(transform.Matrix.Inverse, Direction, 0).Normalize();
        }
        public bool IsSmallerThanCurrent(float distance, Transform trans)
        {
            float newMagnitude = Mattrix.Mul44x41(trans.Matrix, Direction * distance, 0).Magnitude;
            return (newMagnitude < IntersectDistance) ? true : false;
        }
    }
}

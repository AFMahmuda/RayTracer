using System;
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
            Color = new MyColor();
            IntersectDistance = float.MaxValue;
            IntersectWith = null;
        }


        public Point3 Start
        { get; set; }

        public Vector3 Direction
        { get; set; }

        public MyColor Color
        { get; set; }

        public Point3 HitPoint
        {
            get { return Start + (Direction * (IntersectDistance * .999f)).Value; }
        }

        public float IntersectDistance
        { get; set; }

        public Geometry IntersectWith
        { get; set; }

        public MyColor Trace(Scene scene, int bounce)
        {
            if (bounce > scene.MaxDepth)
                return new MyColor();

            foreach (var item in scene.Geometries)
            {
                TransformInv(item.Transform);
                if (item.CheckIntersection(this))
                    IntersectWith = item;
                Transform(item.Transform);
            }

            if (IntersectWith != null)
            {
                Color += CalculateColor(IntersectWith, PopulateEffectiveLight(scene.Lights, scene.Geometries));
                //                Direction = IntersectWith.CalculateReflection(this);
                Ray ray = new Ray();
                ray.Direction = Direction - (IntersectWith.GetNormal(HitPoint) * 2 * (Direction * IntersectWith.GetNormal(HitPoint)));
                ray.Start = HitPoint;
                ray.IntersectDistance = float.MaxValue;
                ray.IntersectWith = null;
                return Color + (.3f * ray.Trace(scene, bounce++));
            }

            else return new MyColor();


        }




        MyColor CalculateColor(Geometry geometry, List<Light> effectiveLights)
        {

            MyColor result = geometry.Ambient + geometry.Material.Emission;
            Vector3 normal = geometry.GetNormal(HitPoint);
            foreach (var item in effectiveLights)
            {
                Vector3 pointToLight = item.GetPointToLight(HitPoint);
                Vector3 halfToLight = (Direction * -1 + pointToLight).Normalize();
                float attenuation = 1;

                result +=
                    attenuation
                    * (geometry.Material.Diffuse * (pointToLight * normal) * item.Color
                    + (geometry.Material.Specular * (float)Math.Pow(halfToLight * normal, geometry.Material.Shininess) * item.Color));
            }
            //            Console.WriteLine(result.R +" "+ result.G + " "+ result.B );
            return result;
        }

        List<Light> PopulateEffectiveLight(List<Light> allLights, List<Geometry> geometries)
        {
            List<Light> result = new List<Light>();
            foreach (var light in allLights)
            {
                if (light.IsEffective(HitPoint, geometries))
                    result.Add(light);
            }

            return result;

        }

        public void Transform(Transform transform)
        {
            Start = Matrix.Mult44x41(transform.Matrix, new Vector3(Start), 1).Value;
            Direction = Matrix.Mult44x41(transform.Matrix, Direction, 0).Normalize();
        }

        public void TransformInv(Transform transform)
        {
            Start = Matrix.Mult44x41(transform.Matrix.Inverse4X4(), new Vector3(Start), 1).Value;
            Direction = Matrix.Mult44x41(transform.Matrix.Inverse4X4(), Direction, 0).Normalize();
        }

        public bool IsSmallerThanCurrent(float t, Transform trans)
        {

            float newMagnitude = Matrix.Mult44x41(trans.Matrix, Direction * t, 0).Magnitude;

            return (newMagnitude < IntersectDistance) ? true : false;
        }



        internal void ShowInformation()
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

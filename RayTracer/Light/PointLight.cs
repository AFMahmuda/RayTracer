using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
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

        public override Vector3 GetPointToLight(Point3 point)
        {
            return new Vector3(point, Position);
        }

        public override bool IsEffective(Point3 point, List<Geometry> geometries)
        {

            Ray ray = new Ray();
            ray.Start = point;
            ray.Direction = GetPointToLight(point);

            foreach (var item in geometries)
            {
                ray.TransformInv(item.Transform);
                if (item.CheckIntersection(ray))
                    if (ray.IntersectDistance < 1)
                        return false;
                ray.Transform(item.Transform);
            }



            return true;
        }

    }
}

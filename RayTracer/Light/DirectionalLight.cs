using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class DirectionalLight : Light
    {


        public DirectionalLight(Vector3 from, MyColor color)
        {
            Direction = from;
            Color = color;

        }
        public DirectionalLight(float[] param)
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
            return new Vector3(point, Direction.Value);
        }

        public override bool IsEffective(Point3 point, List<Geometry> geometries)
        {
            return true;
        }

    }
}

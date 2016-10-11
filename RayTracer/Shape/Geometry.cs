using RayTracer.Common;
using RayTracer.Material;
using RayTracer.Tracer;
using RayTracer.Transformation;
using System;
using System.Collections;

namespace RayTracer.Shape
{
    [Serializable]
    public abstract class Geometry
    {
        public enum TYPE
        {
            SPHERE,
            TRIANGLE
        }

        public TYPE type;


        protected Point3 pos;
        protected bool hasMorton;
        protected uint mortonCode;


        public Point3 Pos { get { return pos; } }
        private Transform trans;
        public Transform Trans
        {
            get { return trans; }
            set { trans = value;
                hasMorton = false;
                UpdatePos();
                }
        }

        public Mat Material;

        public abstract void UpdatePos();
        public abstract bool IsIntersecting(Ray ray);
        public abstract Vec3 GetNormal(Point3 point);
        public MyColor Ambient { get; set; }

        //source : https://devblogs.nvidia.com/parallelforall/thinking-parallel-part-iii-tree-construction-gpu/
        internal uint GetMortonPos()
        {
            if (!hasMorton)
            {
                uint x = expandBits((uint)Math.Min(Math.Max(pos.X * 1024f, 0f), 1023f));
                uint y = expandBits((uint)Math.Min(Math.Max(pos.Y * 1024f, 0f), 1023f));
                uint z = expandBits((uint)Math.Min(Math.Max(pos.Z * 1024f, 0f), 1023f));
                mortonCode = x * 4 + y * 2 + z;
                hasMorton = true;
            }
            return mortonCode;
        }

        uint expandBits(uint v)
        {
            v = (v * 0x00010001u) & 0xFF0000FFu;
            v = (v * 0x00000101u) & 0x0F00F00Fu;
            v = (v * 0x00000011u) & 0xC30C30C3u;
            v = (v * 0x00000005u) & 0x49249249u;
            return v;
        }
        public string GetMortonBitString()
        {
            return Convert.ToString(mortonCode, 2).PadLeft(30, '0');
        }

        public void printPos()
        {
            Console.WriteLine("{3}\t{0}\t{1}\t{2}", pos.X, pos.Y, pos.Z, Convert.ToString(GetMortonPos(), 2).PadLeft(30, '0'));
        }

    }


}

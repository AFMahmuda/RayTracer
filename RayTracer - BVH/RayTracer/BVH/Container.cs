﻿using RayTracer.Shape;
using RayTracer.Tracer;
using System;
using System.Collections.Generic;

namespace RayTracer.BVH
{
    [Serializable]
    public abstract class Container
    {
        public enum TYPE
        { BOX, SPHERE }
        public TYPE Type;

        protected Container[] childs = new Container[2];
        protected Geometry geo = null;
        public float area;

        public abstract bool IsIntersecting(Ray ray);
        public Geometry Geo { get { return geo; } }
        public Container[] Childs { get { return childs; } }

        public float areaWithClosest = float.MaxValue;
        public Container closest = null;

        public void FindBestMatch(List<Container> bins)
        {
            float bestDist = float.MaxValue;
            Container bestmatch = null;
            for (int i = 0; i < bins.Count; i++)
            {
                if (bins[i] == this)
                    continue;
                Container newBin = ContainerFactory.Instance.CombineContainer(this, bins[i]);
                if (newBin.area < bestDist)
                {
                    bestDist = newBin.area;
                    bestmatch = bins[i];
                }
            }
            closest = bestmatch;
            areaWithClosest = bestDist;
        }
    }
}

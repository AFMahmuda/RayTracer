using RayTracer.Tracer;
using System;
using System.Drawing;
using System.IO;

namespace RayTracer
{
    class Program
    {
        static void Main(string[] args)
        {

            String scenefile = "default.test";
            if (args.Length != 0)
                scenefile = args[0];

            if (File.Exists(scenefile))
            {
                TracerManager tracer = new TracerManager(6,true,BVH.Container.TYPE.BOX);
                tracer.TraceScene(scenefile);
            }
            else
            {
                Console.WriteLine("File not found!");
            }

        }
    }
}

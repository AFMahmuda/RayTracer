using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing.Drawing2D;

namespace RayTracer
{


    class Program
    {
        static void Main(string[] args)
        {

            String scenefile = "default.test";
            String outputFile = "default.bmp";
            if (args.Length != 0)
                scenefile = args[0];


            if (File.Exists(scenefile))
            {
                DateTime start = DateTime.Now;
                Console.WriteLine("Preparing File. Please wait...");
                Scene scene = new Scene(scenefile);
                Console.WriteLine("DONE! " + (DateTime.Now - start) + "\n");
                Console.WriteLine();
                RayTracer rayTracer = new RayTracer();
                rayTracer.SuperSampling = false;
                Bitmap result = rayTracer.TraceScene(scene);
                //Bitmap result = rayTracer.TraceScene3D(scene);
                if (!scene.OutputFilename.Equals(""))
                    outputFile = scene.OutputFilename;
                result.Save(outputFile);
                Console.WriteLine("Saved in : " + Directory.GetCurrentDirectory()+"\\"+outputFile);
            }
            else
            {
                Console.WriteLine("File not found!");
            }

            //Console.ReadLine();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //                String scenefile = args[0];
                //                String outputfile = args[1];
                String scenefile = "scene1.test";
                String outputFile = "tralala.bmp";

                if (File.Exists(scenefile))
                {
                    Scene scene = new Scene(scenefile);
                    RayTracer rayTracer = new RayTracer();
                    Bitmap result = rayTracer.TraceScene(scene); 
                    result.Save(outputFile);

                    Console.WriteLine("Success!");
                }
                else
                {
                    Console.WriteLine("File not found!");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("plese insert a valid argument");
            }
            finally
            {
                Console.ReadKey();
            }

        }
    }
}

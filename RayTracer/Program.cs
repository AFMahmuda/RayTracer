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

                String scenefile = "default.test";
                String outputFile = "default.bmp";
                if (args.Length != 0)
                    scenefile = args[0];

                if (File.Exists(scenefile))
                {
                    Scene scene = new Scene(scenefile);
                    RayTracer rayTracer = new RayTracer();
                    Bitmap result = rayTracer.TraceScene(scene);
                    if (!scene.OutputFilename.Equals(""))
                        outputFile = scene.OutputFilename;
                    result.Save(outputFile);

                    Console.WriteLine("Success!");
                }
                else
                {
                    Console.WriteLine("File not found!");
                }
            }
            catch (Exception e)
            {

                Console.WriteLine("plese insert a valid argument" + e.Message);
            }
            finally
            {
                Console.ReadKey();
            }

        }
    }
}

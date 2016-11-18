using RayTracer.Shape;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.BVH
{
    public class ContainerFactory
    {
        static ContainerFactory instance = null;
        public static ContainerFactory Instance
        {
            get
            {
                if (instance == null)
                    instance = new ContainerFactory();
                return instance;
            }

        }

        ContainerFactory()
        {

        }

        public Container CreateContainer(Geometry geo, Container.TYPE type = Container.TYPE.BOX)
        {
            if (type == Container.TYPE.SPHERE)
                return new SphereContainer(geo);
            else// if (type == Container.TYPE.BOX)
                return new BoxContainer(geo);
        }

        public Container CombineContainer(Container a, Container b)
        {
            if (a.Type != b.Type)
                return null;
            else if (a.Type == Container.TYPE.SPHERE)
                return new SphereContainer((SphereContainer)a, (SphereContainer)b);
            else //if (a.Type == Container.TYPE.BOX)
                return new BoxContainer((BoxContainer)a, (BoxContainer)b);
        }

        

    }
}

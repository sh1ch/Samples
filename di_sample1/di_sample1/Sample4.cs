using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Extension;
using static di_sample1.Sample3;

namespace di_sample1
{
    // http://blogs.wankuma.com/nakamura/archive/2008/11/04/160443.aspx
    class Sample4
    {
        public class AnimalExtension : UnityContainerExtension
        {
            protected override void Initialize()
            {
                Container.RegisterType(typeof(IAnimal), typeof(Dog), "Dog", TypeLifetime.Transient);
                Container.RegisterType(typeof(IAnimal), typeof(Cat), "Cat", TypeLifetime.Transient);
            }
        }

        public void Test()
        {
            var container = new UnityContainer();

            container.AddNewExtension<AnimalExtension>();

            var person = container.Resolve<Person>();

            person.Call();
        }
    }
}

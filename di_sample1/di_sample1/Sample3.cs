using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace di_sample1
{
    // http://blogs.wankuma.com/nakamura/archive/2008/10/29/160096.aspx
    class Sample3
    {
        public interface IAnimal
        {
            void Cry();
        }

        public class Cat : IAnimal
        {
            public void Cry() => Console.WriteLine("ニャ～");
        }

        public class Dog : IAnimal
        {
            public void Cry() => Console.WriteLine("バウ！");
        }

        public class Person
        {
            [Dependency("Dog")]
            public IAnimal Pet { get; set; }

            public void Call()
            {
                Pet.Cry();
            }
        }

        public void Test()
        {
            UnityContainer container = new UnityContainer();

            container.RegisterInstance<IAnimal>(nameof(Dog), new Dog());
            container.RegisterInstance<IAnimal>(nameof(Cat), new Cat());

            var person = new Person();

            person = container.BuildUp(person);

            person.Call();
        }
    }
}

using System;
using Unity;
using Unity.Injection;


namespace di_sample1
{
    // https://qiita.com/SY81517/items/adf089d2a800d1929117
    class Sample1
    {
        public interface IVehicle
        {
            int Run();
        }

        public class Car : IVehicle
        {
            private int _Miles = 0;
            public int Run() => ++_Miles;
        }

        public class Bike : IVehicle
        {
            private int _Miles = 0;
            public int Run() => ++_Miles;
        }

        public class Driver
        {
            private IVehicle _Vehicle;

            [InjectionConstructor]
            public Driver(IVehicle vehicle)
            {
                _Vehicle = vehicle;
            }

            public void Run() => Console.WriteLine($"Run {_Vehicle.GetType().Name} - {_Vehicle.Run()} mile.");
        }

        public void Test()
        {
            var container = new UnityContainer();

            container.RegisterType<IVehicle, Car>(nameof(Car));
            container.RegisterType<IVehicle, Bike>(nameof(Bike));

            container.RegisterType<Driver>(nameof(Car) + nameof(Driver), new InjectionConstructor(container.Resolve<IVehicle>(nameof(Car))));
            container.RegisterType<Driver>(nameof(Bike) + nameof(Driver), new InjectionConstructor(container.Resolve<IVehicle>(nameof(Bike))));

            var car = container.Resolve<IVehicle>(nameof(Car));
            var carDriver = container.Resolve<Driver>(nameof(Bike) + nameof(Driver));

            car.Run();
            carDriver.Run();
        }

    }
}

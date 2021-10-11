using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using static di_sample1.Sample1;

namespace di_sample1
{
    // http://blogs.wankuma.com/nakamura/archive/2008/10/28/160029.aspx
    class Sample2
    {
        public interface IMasterManager
        {
            DataSet Read();
        }

        public class CustomerManager : IMasterManager
        {
            public DataSet Read() => new DataSet("Customer");
        }

        public class SupplierManager : IMasterManager
        {
            public DataSet Read() => new DataSet("Supplier");
        }

        public class ProductManager : IMasterManager
        {
            public DataSet Read() => new DataSet("Product");
        }

        public void Test()
        {
            var container = new UnityContainer();

            container.RegisterInstance<IMasterManager>("Customer", new CustomerManager());
            container.RegisterInstance<IMasterManager>("Supplier", new SupplierManager());
            container.RegisterInstance<IMasterManager>(new ProductManager());

            var manager1 = container.Resolve<IMasterManager>("Customer");
            var manager2 = container.Resolve<IMasterManager>("Supplier");
            var manager3 = container.Resolve<IMasterManager>();

            // ERROR ここからは呼び出せない
            // var manager4 = container.Resolve<IVehicle>("Bike");

            Console.WriteLine(manager1.Read().DataSetName);
            Console.WriteLine(manager2.Read().DataSetName);
            Console.WriteLine(manager3.Read().DataSetName);

            // 1, 2 だけ
            var managers = container.ResolveAll<IMasterManager>();

            foreach (var manager in managers)
            {
                Console.WriteLine(manager.Read().DataSetName);
            }
        }
    }
}

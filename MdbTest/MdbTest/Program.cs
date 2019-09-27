using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MdbTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();

            program.Run();
        }

        public void Run()
        {
            var db = new Mdb();
            
            Console.WriteLine($"ADO Engine タイプ : {db.GetEngineType()}");
            Console.WriteLine($"DAO Access バージョン : {db.GetAccessVersion()}");
            Console.WriteLine($"推定 Access バージョン : {db.GetAccessType()}");

            Console.WriteLine($"----");

            var charactors = db.SelectFromOLEDB();

            foreach (var chara in charactors)
            {
                Console.WriteLine($"名前:{chara.Name.TrimEnd()} HP:{chara.Hp, 4} MP:{chara.Mp, 4} 状態:{(!chara.IsDead ? "生": "死")}");
            }
        }
    }
}

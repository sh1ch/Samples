using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MdbTest
{
    /// <summary>
    /// <see cref="Charactor"/> クラスは、サンプルテーブルのテストデータを表現します。
    /// </summary>
    public class Charactor
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? Hp { get; set; }

        public int? Mp { get; set; }

        public bool IsDead { get; set; }

    }
}

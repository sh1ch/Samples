using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGridCustomCheckBoxTest
{
    public class SampleData
    {
        public int No { get; set; }

        public string Text { get; set; }

        private bool _Data1;

        public bool Data1
        {
            get => _Data1;
            set
            {
                Console.WriteLine($"{nameof(Data1)} Setter Called.");
                _Data1 = value;
            }
        }

        private bool _Data2;
        public bool Data2
        {
            get => _Data2;
            set
            {
                Console.WriteLine($"{nameof(Data2)} Setter Called.");
                _Data2 = value;
            }
        }
    }
}

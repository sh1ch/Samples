using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PasswordBoxTest
{
    public class MainWindowViewModel : BindableBase
    {
        private string _text1 = "test value1";
        public string Text1 
        {
            get => _text1;
            set => SetProperty(ref _text1, value);
        }

        private string _text2 = "test value2";
        public string Text2
        {
            get => _text2;
            set => SetProperty(ref _text2, value);
        }

        public DelegateCommand ResetTextBoxCommand { get; private set; }


        public MainWindowViewModel()
        {
            ResetTextBoxCommand = new DelegateCommand(() =>
            {
                Text1 = "reset value";
            });
        }

    }
}

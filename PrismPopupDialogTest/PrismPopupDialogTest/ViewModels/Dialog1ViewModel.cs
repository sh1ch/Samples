using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismPopupDialogTest.ViewModels
{
    public class Dialog1ViewModel : BindableBase, IDialogAware
    {
        private string _Title = "Test Title";

        public event Action<IDialogResult> RequestClose;

        public string Title
        {
            get => _Title;
            set => SetProperty(ref _Title, value);
        }

        private string _Sample1;

        public string Sample1
        {
            get => _Sample1;
            set => SetProperty(ref _Sample1, value);
        }

        private string _Sample2;

        public string Sample2
        {
            get => _Sample2;
            set => SetProperty(ref _Sample2, value);
        }

        private int _Sample3;

        public int Sample3
        {
            get => _Sample3;
            set => SetProperty(ref _Sample3, value);
        }

        public DelegateCommand CloseCommand { get; }
        public DelegateCommand UpValueCommand { get; }
        public DelegateCommand DownValueCommand { get; }

        /// <summary>
        /// <see cref="Dialog1ViewModel"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public Dialog1ViewModel()
        {
            CloseCommand = new (() =>
            {
                var result = new DialogResult();

                result.Parameters.Add("key3", Sample3);

                RequestClose?.Invoke(result);

                Debug.WriteLine($"{nameof(CloseCommand)} called.");
            });

            UpValueCommand = new (() =>
            {
                Sample3++;
            });

            DownValueCommand = new (() =>
            {
                Sample3--;
            });

        }

        public bool CanCloseDialog() => true;

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Sample1 =  parameters.GetValue<string>("key1") ?? "error";
            Sample2 =  parameters.GetValue<string>("key2") ?? "error";
            Sample3 =  parameters.GetValue<int>("key3");

            Debug.WriteLine($"{nameof(OnDialogOpened)} called.");
        }

        public void OnDialogClosed()
        {

        }

    }
}

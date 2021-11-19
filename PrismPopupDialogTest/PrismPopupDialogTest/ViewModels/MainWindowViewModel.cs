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
    public class MainWindowViewModel : BindableBase
    {
        private IDialogService _DialogService;
        private string _Title = "Prism - PopupDialog Test";

        public string Title
        {
            get => _Title;
            set => SetProperty(ref _Title, value);
        }

        private int _SampleValue = 3;

        public int SampleValue
        {
            get => _SampleValue;
            set => SetProperty(ref _SampleValue, value);
        }

        public DelegateCommand PopupDialog1Command { get; }
        public DelegateCommand PopupDialog2Command { get; }

        /// <summary>
        /// <see cref="MainWindowViewModel"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public MainWindowViewModel(IDialogService dialogService)
        {
            _DialogService = dialogService;

            PopupDialog1Command = new (() =>
            {
                Debug.WriteLine($"{nameof(PopupDialog1Command)} called.");
                
                var parameters = new DialogParameters
                {
                    { "key1", "send data1" },
                    { "key2", "send data2" },
                    { "key3", SampleValue },
                };

                _DialogService.ShowDialog("Dialog1", parameters, (results) => 
                {
                    SampleValue = results.Parameters.GetValue<int>("key3");

                    Debug.WriteLine($"Dialog1 result called.");
                });
            });

            PopupDialog2Command = new(() =>
            {
                Debug.WriteLine($"{nameof(PopupDialog2Command)} called.");

                var parameters = new DialogParameters
                {
                    { "key1", "send data1" },
                    { "key2", "send data2" },
                    { "key3", SampleValue },
                };

                _DialogService.ShowDialog("Dialog2", parameters, (results) =>
                {
                    SampleValue = results.Parameters.GetValue<int>("key3");

                    Debug.WriteLine($"Dialog2 result called.");
                });
            });

        }
    }
}

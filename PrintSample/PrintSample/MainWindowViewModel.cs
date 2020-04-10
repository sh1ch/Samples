using HeritageFramework.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PrintSample
{
    /// <summary>
    /// <see cref="MainWindowViewModel"/> クラスは、<see cref="MainWindow"/> の VM クラスです。
    /// </summary>
    public class MainWindowViewModel
    {
        #region Properties

        public PrintSettings PrintSettings { get; } = new PrintSettings();

        public SampleOption SampleOption { get; } = new SampleOption();

        public SamplePreview Preview { get; } = new SamplePreview();

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="MainWindowViewModel"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public MainWindowViewModel() 
        {
            RunMemoryChecker();
            UpdatePreviewInDispatcher();

            SampleOption.Changed += (sender, args) => 
            {
                UpdatePreviewInDispatcher();
            };

            PrintSettings.PrinterChanged += (sender, args) =>
            {
                UpdatePreviewInDispatcher();
            };

        }

        #endregion

        #region Events

        private RelayCommand<CommandInfoArgs> _PrintCommand;

        public RelayCommand<CommandInfoArgs> PrintCommand
        {
            get
            {
                return _PrintCommand = _PrintCommand ?? new RelayCommand<CommandInfoArgs>((args) =>
                {
                    Preview.Print(PrintSettings);
                });
            }
        }

        private RelayCommand<CommandInfoArgs> _OpenOsSettingsCommand;

        public RelayCommand<CommandInfoArgs> OpenOsSettingsCommand
        {
            get
            {
                return _OpenOsSettingsCommand = _OpenOsSettingsCommand ?? new RelayCommand<CommandInfoArgs>((args) =>
                {
                    PrintSettings.OpenMsSettings();
                });
            }
        }

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        private void UpdatePreviewInDispatcher()
        {
            var dispatcher = Application.Current.Dispatcher;
            
            dispatcher.BeginInvoke(new Action(() => 
            {
                Preview.UpdateCapabilities(PrintSettings);
                Preview.UpdateDocument(SampleOption);
            }));
        }

        private void RunMemoryChecker()
        {
            var dispatcher = Application.Current.Dispatcher;

            var thread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);

                    Console.WriteLine("Total Memory = {0} KB", GC.GetTotalMemory(true) / 1024);
                }
            })
            {
                IsBackground = true,
            };

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        #endregion
    }
}

using AreaCalculator.Models;
using HeritageFramework.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AreaCalculator.ViewModels
{
    public class MainWindowViewModel
    {
        #region Properties

        public PeakAreaPlot PeakPlot { get; } = new PeakAreaPlot();
        public PeakFile File { get; } = new PeakFile();

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="MainWindowViewModel"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public MainWindowViewModel()
        {
            File.DataPointUpdated += (sender, args) => 
            {
                var points = args.Value;

                // ピークの表を初期化する
                if (points != null && points.Count() > 0)
                {
                    PeakPlot.InitializeSeries(points);
                }
                else
                {
                    PeakPlot.ClearSeries();
                }
            };
        }

        #endregion

        #region Events

        private RelayCommand<CommandInfoArgs> _GetPeakAreaCommand;

        public RelayCommand<CommandInfoArgs> GetPeakAreaCommand
        {
            get
            {
                return _GetPeakAreaCommand = _GetPeakAreaCommand ?? new RelayCommand<CommandInfoArgs>(GetPeakArea);
            }
        }

        private void GetPeakArea(CommandInfoArgs args)
        {

        }

        #endregion

        #region Commands

        private RelayCommand<CommandInfoArgs<DragEventArgs>> _DragOverCsvFileCommand;

        public RelayCommand<CommandInfoArgs<DragEventArgs>> DragOverCsvFileCommand
        {
            get
            {
                return _DragOverCsvFileCommand = _DragOverCsvFileCommand ?? new RelayCommand<CommandInfoArgs<DragEventArgs>>(DragOverCsvFile);
            }
        }

        private void DragOverCsvFile(CommandInfoArgs<DragEventArgs> args)
        {
            var dragArgs = args.EventArgs;
            var effect = DragDropEffects.None;

            if (dragArgs.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                var files = (string[])dragArgs.Data.GetData(DataFormats.FileDrop, false);

                if (files.Count() == 1)
                {
                    dragArgs.Effects = DragDropEffects.Copy;
                }
            }

            dragArgs.Effects = effect;
        }

        private RelayCommand<CommandInfoArgs<DragEventArgs>> _DropCsvFileCommand;

        public RelayCommand<CommandInfoArgs<DragEventArgs>> DropCsvFileCommand
        {
            get
            {
                return _DropCsvFileCommand = _DropCsvFileCommand ?? new RelayCommand<CommandInfoArgs<DragEventArgs>>(DropCsvFile);
            }
        }

        private void DropCsvFile(CommandInfoArgs<DragEventArgs> args)
        {
            var dragArgs = args.EventArgs;

            var files = (string[])dragArgs.Data.GetData(DataFormats.FileDrop, false);

            // 単体のファイル選択
            if (files.Count() == 1)
            {
                var selectedFile = files[0];

                // ファイルであること、拡張子が txt or csv
                if (System.IO.File.Exists(selectedFile) && 
                   (Path.GetExtension(selectedFile).ToLower() == ".txt" || Path.GetExtension(selectedFile).ToLower() == ".csv"))
                {
                    File.Parse(selectedFile);
                }
            }
        }

        #endregion

        #region Public Methods

        #endregion
    }
}

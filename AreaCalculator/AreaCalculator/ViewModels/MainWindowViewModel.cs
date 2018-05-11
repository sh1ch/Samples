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
        public PeakCalculationParameter Parameter { get; } = new PeakCalculationParameter();
        public PeakResult Result { get; } = new PeakResult();
        public PeakOption Option { get; } = new PeakOption();

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
                    // ピークを評価
                    Result.MeasurePeakPoints(points, Parameter.StartFactor, Parameter.EndFactor, Parameter.FactorMinSeconds);
                    Result.UpdatePeakArea(points);

                    // オプションを更新
                    Option.UpdateSymmetryFactor(points, Result);
                    Option.UpdateTheoreticalPlateNumber(points, Result);

                    // グラフを更新
                    PeakPlot.InitializeSeries1(points);
                    PeakPlot.InitializeSeries2(points, Result);

                    PeakPlot.UpdatePeakPoints(Result);
                    PeakPlot.Update();
                }
                else
                {
                    PeakPlot.ClearSeries();
                }
            };

            Result.PropertyChanged += (sender, args) =>
            {
                if (Result.IsEnabled)
                {
                    var startSeconds = Result.StartPoint.X;
                    var endSeconds = Result.EndPoint.X;

                    Parameter.StartSeconds = startSeconds;
                    Parameter.EndSeconds = endSeconds;
                }
            };
        }

        #endregion

        #region Commands

        private RelayCommand<CommandInfoArgs> _CopyToClipboardCommand;

        public RelayCommand<CommandInfoArgs> CopyToClipboardCommand
        {
            get
            {
                return _CopyToClipboardCommand = _CopyToClipboardCommand ?? new RelayCommand<CommandInfoArgs>(CopyToClipboard);
            }
        }

        private void CopyToClipboard(CommandInfoArgs args)
        {
            var text = args.Parameter?.ToString() ?? "";

            Clipboard.SetText(text);
        }

        private RelayCommand<CommandInfoArgs> _CalculatePeakPointsFromFactorCommand;

        public RelayCommand<CommandInfoArgs> CalculatePeakPointsFromFactorCommand
        {
            get
            {
                return _CalculatePeakPointsFromFactorCommand = _CalculatePeakPointsFromFactorCommand ?? new RelayCommand<CommandInfoArgs>(CalculatePeakPointsFromFactor);
            }
        }

        private void CalculatePeakPointsFromFactor(CommandInfoArgs args)
        {
            var points = File.Points;
            if ((points?.Count() ?? 0) <= 0) return;

            // ピークを評価
            Result.MeasurePeakPoints(points, Parameter.StartFactor, Parameter.EndFactor, Parameter.FactorMinSeconds);
            Result.UpdatePeakArea(points);

            // グラフを更新
            PeakPlot.InitializeSeries2(points, Result);

            PeakPlot.UpdatePeakPoints(Result);
            PeakPlot.Update();
        }

        private RelayCommand<CommandInfoArgs> _CalculatePeakPointsFromSecondCommand;

        public RelayCommand<CommandInfoArgs> CalculatePeakPointsFromSecondCommand
        {
            get
            {
                return _CalculatePeakPointsFromSecondCommand = _CalculatePeakPointsFromSecondCommand ?? new RelayCommand<CommandInfoArgs>(CalculatePeakPointsFromSecond);
            }
        }

        private void CalculatePeakPointsFromSecond(CommandInfoArgs args)
        {
            var points = File.Points;
            if ((points?.Count() ?? 0) <= 0) return;

            var startSeconds = Parameter.StartSeconds;
            var endSeconds = Parameter.EndSeconds;

            // 時間の指定が逆のとき、入れ替えて計算
            if (endSeconds < startSeconds)
            {
                var temp = startSeconds;

                startSeconds = endSeconds;
                endSeconds = temp;
            }

            Result.ForceSetPeakPoint(points, startSeconds, endSeconds);
            Result.UpdatePeakArea(points);

            // グラフを更新
            PeakPlot.InitializeSeries2(points, Result);

            PeakPlot.UpdatePeakPoints(Result);
            PeakPlot.Update();
        }

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
                    File.Parse(selectedFile, Parameter.SignalSelectionType);
                }
            }
        }

        private RelayCommand<CommandInfoArgs> _CloseWindowCommand;

        public RelayCommand<CommandInfoArgs> CloseWindowCommand
        {
            get
            {
                return _CloseWindowCommand = _CloseWindowCommand ?? new RelayCommand<CommandInfoArgs>(CloseWindow);
            }
        }

        private void CloseWindow(CommandInfoArgs args)
        {
            Parameter.Save();
        }

        #endregion

        #region Public Methods

        #endregion
    }
}

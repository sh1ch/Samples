using AreaCalculator.Models.SignalConverter;
using AreaCalculator.Models.SignalSelection;
using HeritageFramework;
using HeritageFramework.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaCalculator.Models
{
    /// <summary>
    /// <see cref="PeakFile"/> クラスは、ピークの信号データを解析するクラスです。
    /// </summary>
    public class PeakFile : INotifyPropertyChanged
    {
        #region Properties

        private string _SelectedPath;

        /// <summary>
        /// 選択したファイルパスを取得します。
        /// </summary>
        public string SelectedPath
        {
            get { return _SelectedPath; }
            private set
            {
                _SelectedPath = value;
                PropertyChanged.Raise(this, nameof(SelectedPath));
            }
        }

        private List<DataPoint> _Points = new List<DataPoint>();

        /// <summary>
        /// 信号強度の列挙子を取得します。
        /// </summary>
        public IEnumerable<DataPoint> Points
        {
            get { return _Points; }
        }

        /// <summary>
        /// データ数を取得します。
        /// </summary>
        public int DataCount
        {
            get { return _Points.Count; }
        }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="PeakFile"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public PeakFile()
        {
            
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<GenericEventArgs<IEnumerable<DataPoint>>> DataPointUpdated;

        #endregion

        #region Public Methods

        /// <summary>
        /// 指定したファイルパスのファイルのテキストを解析します。
        /// <para>
        /// 解析に失敗したときは <see cref="SelectedPath"/> を "" に変更します。
        /// </para>
        /// </summary>
        /// <param name="path">ファイルパス。</param>
        /// <param name="type">シグナルの取得方法。</param>
        public void Parse(string path, SignalSelectionType type)
        {
            _Points.Clear();

            try
            {
                using (var reader = new StreamReader(path, Encoding.UTF8))
                {
                    var converter = new SignalConverterV1();
                    var dataSelector = GetSignalSelection(type);
                    var line = "";
                    int sec = 0;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(line)) continue;

                        var data = line.Split(',');

                        sec = int.Parse(data[0]);
                        var signals = dataSelector.Select(data.Skip(1), 10);

                        for (var i = 0; i < signals.Count(); i++)
                        {
                            var point = new DataPoint(sec + 0.1 * i, signals[i], converter);
                            _Points.Add(point);
                        }
                    }
                }

                // 前後のデータ ポイントを設定
                var points = _Points;
                DataPoint prevP = null;
                DataPoint nowP = null;
                DataPoint nextP = null;

                for (var i = 0; i < points.Count(); i++)
                {
                    prevP = nowP;
                    nowP = points.ElementAtOrDefault(i);
                    nextP = points.ElementAtOrDefault(i + 1);

                    nowP.Previous = prevP;
                    nowP.Next = nextP;
                }

                SelectedPath = path;
            }
            catch
            {
                SelectedPath = "";
            }

            DataPointUpdated?.Invoke(this, GenericEventArgs.Create(Points));
            PropertyChanged.Raise(this, nameof(DataCount));
        }

        #endregion

        #region Private Methods

        private ISignalSelection GetSignalSelection(SignalSelectionType type)
        {
            ISignalSelection selection = null;

            switch (type)
            {
                case SignalSelectionType.Simple:
                    selection = new SimpleSignalSelection();
                    break;
                case SignalSelectionType.ChangingPoint:
                    selection = new ChangingSignalSelection();
                    break;
                default:
                    selection = new SimpleSignalSelection();
                    break;
            }

            return selection;
        }

        #endregion
    }
}

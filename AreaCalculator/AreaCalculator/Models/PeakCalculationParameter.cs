using AreaCalculator.Models.SignalSelection;
using HeritageFramework;
using HeritageFramework.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaCalculator.Models
{
    /// <summary>
    /// <see cref="PeakCalculationParameter"/> クラスは、ピーク算出するためのパラメーター クラスです。
    /// </summary>
    public class PeakCalculationParameter : INotifyPropertyChanged
    {
        #region Properties

        private SignalSelectionType _SignalSelectionType;

        /// <summary>
        /// シグナルを選択する種別を取得または設定します。
        /// </summary>
        public SignalSelectionType SignalSelectionType
        {
            get { return _SignalSelectionType; }
            set
            {
                _SignalSelectionType = value;
                PropertyChanged.Raise(this, nameof(SignalSelectionType));
            }
        }

        private double _StartSeconds;

        /// <summary>
        /// 開始時間を取得または設定します。
        /// </summary>
        public double StartSeconds
        {
            get { return _StartSeconds; }
            set
            {
                _StartSeconds = value;
                PropertyChanged.Raise(this, nameof(StartSeconds));
            }
        }

        private double _EndSeconds;

        /// <summary>
        /// 終了時間を取得または設定します。
        /// </summary>
        public double EndSeconds
        {
            get { return _EndSeconds; }
            set
            {
                _EndSeconds = value;
                PropertyChanged.Raise(this, nameof(EndSeconds));
            }
        }

        private double _StartFactor;

        /// <summary>
        /// 開始条件値を取得または設定します。
        /// </summary>
        public double StartFactor
        {
            get { return _StartFactor; }
            set
            {
                _StartFactor = value;
                PropertyChanged.Raise(this, nameof(StartFactor));
            }
        }

        private double _EndFactor;

        /// <summary>
        /// 終了条件値を取得または設定します。
        /// </summary>
        public double EndFactor
        {
            get { return _EndFactor; }
            set
            {
                _EndFactor = value;
                PropertyChanged.Raise(this, nameof(EndFactor));
            }
        }

        private double _FactorMinSeconds;

        /// <summary>
        /// 面積を算出する最小幅を取得または設定します。
        /// </summary>
        public double FactorMinSeconds
        {
            get { return _FactorMinSeconds; }
            set
            {
                _FactorMinSeconds = value;
                PropertyChanged.Raise(this, nameof(FactorMinSeconds));
            }
        }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="PeakCalculationParameter"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public PeakCalculationParameter()
        {
            Load();
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Methods

        public void Load()
        {
            var settings = Properties.Settings.Default;

            StartFactor = settings.StartFactor;
            EndFactor = settings.EndFactor;
            FactorMinSeconds = settings.FactorMinSeconds;

            StartSeconds = settings.StartSeconds;
            EndSeconds = settings.EndSeconds;

            SignalSelectionType = (SignalSelectionType)settings.SignalSelectionType;
        }

        public void Save()
        {
            var settings = Properties.Settings.Default;

            settings.StartFactor = StartFactor;
            settings.EndFactor = EndFactor;
            settings.FactorMinSeconds = FactorMinSeconds;

            settings.StartSeconds = StartSeconds;
            settings.EndSeconds = EndSeconds;

            settings.SignalSelectionType = (int)SignalSelectionType;

            settings.Save();
        }

        #endregion
    }
}

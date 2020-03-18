using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGridColumnPositionSaveSample
{
    public class MainWindowViewModel
    {
        #region Properties

        public ObservableCollection<SampleData> Records { get; } = new ObservableCollection<SampleData>();

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="MainWindowViewModel"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public MainWindowViewModel() 
        {
            Records.Add(new SampleData { No = 1, Name = "ランス", Type = "戦士", Hp = 100, Mp = 0 });
            Records.Add(new SampleData { No = 2, Name = "シィル", Type = "魔法使い", Hp = 10, Mp = 90 });
            Records.Add(new SampleData { No = 3, Name = "パットン", Type = "戦士", Hp = 200, Mp = 0 });
            Records.Add(new SampleData { No = 4, Name = "リック", Type = "戦士", Hp = 80, Mp = 0 });
            Records.Add(new SampleData { No = 5, Name = "鈴女", Type = "忍者", Hp = 50, Mp = 50 });
            Records.Add(new SampleData { No = 6, Name = "あてな２号", Type = "遊び人", Hp = 20, Mp = 40 });
        }

        #endregion
    }
}

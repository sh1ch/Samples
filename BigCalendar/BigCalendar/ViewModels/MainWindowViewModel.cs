using BigCalendar.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigCalendar.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
	private static readonly Random _Random = new Random();
	private static readonly TimeProvider _TimeProvider = TimeProvider.System;

	public ObservableCollection<Data> Samples { get; set; } = new ObservableCollection<Data>();

	/// <summary>
	/// <see cref="MainWindowViewModel"/> クラスの新しいインスタンスを初期化します。
	/// </summary>
	public MainWindowViewModel()
	{
	}

	[RelayCommand]
	public void AddSample(CustomCalendar calendar)
	{
		var now = _TimeProvider.GetLocalNow();
		var newDate = new DateTimeOffset(now.Year, now.Month, now.Day, 0, 0, 0, now.Offset);

		Samples.Add(new Data { Description = "ABC", Date = newDate });

		// ForceUpdate を呼ばないとカレンダーが更新されない
		calendar.ForceUpdate();
	}
}

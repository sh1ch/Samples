using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioEnumBindingTest;

public partial class MainWindowViewModel : ObservableObject
{
	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(Text))]
	private SampleEnum _sample = SampleEnum.A;

	public string Text => $"{Sample}.";

	/// <summary>
	/// <see cref="MainWindowViewModel"/> クラスの新しいインスタンスを初期化します。
	/// </summary>
	public MainWindowViewModel()
	{
	}
}

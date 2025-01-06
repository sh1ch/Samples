using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BigCalendar.Views;

/// <summary>
/// CustomCalendar.xaml の相互作用ロジック
/// </summary>
public partial class CustomCalendar : UserControl
{
	public static readonly DependencyProperty EventsProperty =
		DependencyProperty.Register
		(
			nameof(Events),
			typeof(ObservableCollection<Data>),
			typeof(CustomCalendar),
			new PropertyMetadata
			(
				new ObservableCollection<Data>()
			));

	public ObservableCollection<Data> Events
	{
		get => (ObservableCollection<Data>)GetValue(EventsProperty);
		set => SetValue(EventsProperty, value);
	}

	public CustomCalendar()
	{
		InitializeComponent();
	}

	public void ForceUpdate()
	{
		var selectedMode = CalendarControl.DisplayMode;

		CalendarControl.DisplayMode = CalendarMode.Year;
		CalendarControl.DisplayMode = selectedMode;
	}
}

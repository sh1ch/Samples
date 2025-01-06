using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace BigCalendar.Views;

public class CalendarB : Calendar
{
	protected override AutomationPeer? OnCreateAutomationPeer()
	{
		// 空の AutomationPeer を返すことで Automation サポートを無効化
		return null;
	}
}
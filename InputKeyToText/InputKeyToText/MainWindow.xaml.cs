using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InputKeyToText
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		[DllImport("user32.dll")]
		private static extern int ToUnicode(uint virtualKeyCode, uint scanCode, byte[] keyboardState,
        [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder receivingBuffer, int bufferSize, uint flags);

		[DllImport("user32.dll")]
		private static extern bool GetKeyboardState(byte[] keyboardState);

		[DllImport("user32.dll")]
		private static extern uint MapVirtualKey(uint uCode, uint uMapType);

		private string KeyToUnicode(Key key)
		{
			var virtualKey = KeyInterop.VirtualKeyFromKey(key);
			var scanCode = MapVirtualKey((uint)virtualKey, 0);
			var keyboardState = new byte[256];

			GetKeyboardState(keyboardState);

			var sb = new StringBuilder(2);
			var result = ToUnicode((uint)virtualKey, scanCode, keyboardState, sb, sb.Capacity, 0);

			return sb.ToString();
		}

		public MainWindow()
        {
            InitializeComponent();
        }

		private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			var input = KeyToUnicode(e.Key);

			if (string.IsNullOrEmpty(input)) 
			{
				input = "input is '' (empty).";
			}

			TextBlock01.Text = $"convert = {input}";
			TextBlock02.Text = $"e.Key = {e.Key.ToString()}";
		}
	}
}
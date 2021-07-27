using System;
using System.Collections.Generic;
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

namespace CoreAudioManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            UpdateStatus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsNewMuted.IsChecked ?? false)
            {
                AudioManager.SetMasterMuteVolume(true);
            }
            else
            {
                if (AudioManager.GetMasterMuteVolumeMute())
                {
                    AudioManager.SetMasterMuteVolume(false);
                }

                var newVolume = (float)NewMainVolume.Value;

                AudioManager.SetMasterVolume(newVolume);
            }

            UpdateStatus();
        }

        private void UpdateStatus()
        {
            MainVolume.Content = AudioManager.GetMasterVolume().ToString("");
            IsMuted.Content = AudioManager.GetMasterMuteVolumeMute().ToString();
        }
    }
}

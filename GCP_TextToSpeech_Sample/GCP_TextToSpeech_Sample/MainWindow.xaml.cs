using Google.Cloud.TextToSpeech.V1;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace GCP_TextToSpeech_Sample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Media.SoundPlayer _Player;
        TextToSpeechClient _Client;

        public MainWindow()
        {
            _Client = TextToSpeechClient.Create();

            InitializeComponent();
        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            var text = SpeechText.Text;
            var name = (UseWaveNet.IsChecked ?? false) ? "ja-JP-Wavenet-A" : "ja-JP-Standard-A";
            var speed = SpeedSlider.Value;
            var pitch = PitchSlider.Value;

            var input = new SynthesisInput { Text = text };
            var voiceSection = new VoiceSelectionParams
            {
                Name = name,
                LanguageCode = "ja-JP",
                SsmlGender = SsmlVoiceGender.Female,
            };
            var audioConfig = new AudioConfig
            {
                AudioEncoding = AudioEncoding.Mp3,
                SpeakingRate = speed,
                Pitch = pitch,
            };

            var response = _Client.SynthesizeSpeech(input, voiceSection, audioConfig);

            using (var output = File.Create("output.mp3"))
            {
                response.AudioContent.WriteTo(output);
            }
        }

        private void Button_Click_Play(object sender, RoutedEventArgs e)
        {
            var text = SpeechText.Text;
            var name = (UseWaveNet.IsChecked ?? false) ? "ja-JP-Wavenet-A" : "ja-JP-Standard-A";
            var speed = SpeedSlider.Value;
            var pitch = PitchSlider.Value;

            if (_Player != null)
            {
                _Player?.Stop();
                _Player?.Dispose();
            }

            var input = new SynthesisInput { Text = text };
            var voiceSection = new VoiceSelectionParams
            {
                Name = name,
                LanguageCode = "ja-JP",
                SsmlGender = SsmlVoiceGender.Female,
            };
            var audioConfig = new AudioConfig
            {
                AudioEncoding = AudioEncoding.Linear16,
                SpeakingRate = speed,
                Pitch = pitch,
            };

            var response = _Client.SynthesizeSpeech(input, voiceSection, audioConfig);

            using (var memoryStream = new MemoryStream(response.AudioContent.ToArray(), true))
            {
                _Player = new System.Media.SoundPlayer(memoryStream);
                _Player.Play();
            }
        }
    }
}

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

            var settings = Properties.Settings.Default;

            // 設定の読み込み
            UseWaveNet.IsChecked = settings.UseWaveNet;
            SpeedSlider.Value = settings.Speed;
            PitchSlider.Value = settings.Pitch;

            UseAutoIncrement.IsChecked = settings.UseAutoIncrement;
            SceneID.Text = settings.SceneID;
            TextID.Text = $"{settings.TextID:000}";
            RetakeID.Text = settings.RetakeID;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var settings = Properties.Settings.Default;

            // 設定の保存
            settings.UseWaveNet = UseWaveNet.IsChecked ?? false;
            settings.Speed = SpeedSlider.Value;
            settings.Pitch = PitchSlider.Value;

            settings.UseAutoIncrement = UseAutoIncrement.IsChecked ?? false;
            settings.SceneID = SceneID.Text;
            settings.TextID = Convert.ToInt32(TextID.Text);
            settings.RetakeID = RetakeID.Text;

            settings.Save();
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
            var fileName = GetFileName();

            using (var output = File.Create($"mp3\\{fileName}"))
            {
                response.AudioContent.WriteTo(output);
            }

            // 画面を更新
            UpdateDisplayValues(fileName);
        }

        private string GetFileName()
        {
            var sceneID = SceneID.Text;
            var textID = TextID.Text;
            var retakeID = RetakeID.Text;
            var text = SpeechText.Text.Length > 5 ? SpeechText.Text.Substring(0, 5) : SpeechText.Text;

            return $"{sceneID}{textID}-{retakeID}_{text}.mp3";
        }

        private void UpdateDisplayValues(string fileName)
        {
            if (UseAutoIncrement.IsChecked ?? false)
            {
                var textID = Convert.ToInt32(TextID.Text) + 1;
                TextID.Text = $"{textID:000}";
            }
            
            ResultText.Content = $"{fileName} の音声データを保存しました。";
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

        private void Button_Click_Zero(object sender, RoutedEventArgs e)
        {
            TextID.Text = $"{1:000}";
        }
    }
}

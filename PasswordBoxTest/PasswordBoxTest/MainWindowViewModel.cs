using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials.UI;
using Windows.UI.ApplicationSettings;

namespace PasswordBoxTest
{
    public class MainWindowViewModel : BindableBase
    {
        private string _text1 = "test value1";
        public string Text1 
        {
            get => _text1;
            set => SetProperty(ref _text1, value);
        }

        private string _text2 = "test value2";
        public string Text2
        {
            get => _text2;
            set => SetProperty(ref _text2, value);
        }

        private UserConsentVerificationResult? _result;
        public UserConsentVerificationResult? Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        public DelegateCommand ResetTextBoxCommand { get; }

        public DelegateCommand TestFingerPrintCommand { get; }

        public DelegateCommand TestWebAccountCommand { get; }


        public MainWindowViewModel()
        {
            ResetTextBoxCommand = new DelegateCommand(() =>
            {
                Text1 = "reset value";
            });

            TestFingerPrintCommand = new DelegateCommand(async () =>
            {
                var ucvAvailability = await UserConsentVerifier.CheckAvailabilityAsync();

                if (ucvAvailability == UserConsentVerifierAvailability.Available)
                {
                    var consentResult = await UserConsentVerifier.RequestVerificationAsync("Please provide fingerprint verification.");

                    if (consentResult == UserConsentVerificationResult.Verified)
                    {
                        Debug.WriteLine("OK");
                    }

                    Result = consentResult;
                }
            });

            TestWebAccountCommand = new DelegateCommand(() =>
            {
                AccountsSettingsPane.Show();
            });

        }

    }
}

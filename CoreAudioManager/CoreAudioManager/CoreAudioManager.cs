using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CoreAudioManager
{
    public static class AudioManager
    {
        public static float GetMasterVolume()
        {
            IAudioEndpointVolume masterVolume = null;

            try
            {
                masterVolume = GetMasterVolumeObject();

                if (masterVolume == null)
                {
                    return -1;
                }

                masterVolume.GetMasterVolumeLevelScalar(out float volumeLevel);

                return volumeLevel * 100;
            }
            finally
            {
                if (masterVolume != null)
                {
                    Marshal.ReleaseComObject(masterVolume);
                }
            }
        }

        public static bool GetMasterMuteVolumeMute()
        {
            IAudioEndpointVolume masterVolume = null;

            try
            {
                masterVolume = GetMasterVolumeObject();

                if (masterVolume == null)
                {
                    return false;
                }

                masterVolume.GetMute(out bool isMuted);

                return isMuted;
            }
            finally
            {
                if (masterVolume != null)
                {
                    Marshal.ReleaseComObject(masterVolume);
                }
            }
        }

        public static void SetMasterVolume(float newLevel)
        {
            IAudioEndpointVolume masterVol = null;

            try
            {
                masterVol = GetMasterVolumeObject();

                if (masterVol == null)
                {
                    return;
                }

                masterVol.SetMasterVolumeLevelScalar(newLevel / 100, Guid.Empty);
            }
            finally
            {
                if (masterVol != null)
                {
                    Marshal.ReleaseComObject(masterVol);
                }
            }
        }

        public static void SetMasterMuteVolume(bool isMuted)
        {
            IAudioEndpointVolume masterVolume = null;
            try
            {
                masterVolume = GetMasterVolumeObject();

                if (masterVolume == null)
                {
                    return;
                }

                masterVolume.SetMute(isMuted, Guid.Empty);
            }
            finally
            {
                if (masterVolume != null)
                {
                    Marshal.ReleaseComObject(masterVolume);
                }
            }
        }

        #region Private Methods

        private static IAudioEndpointVolume GetMasterVolumeObject()
        {
            IMMDeviceEnumerator deviceEnumerator = null;
            IMMDevice speakers = null;

            try
            {
                deviceEnumerator = (IMMDeviceEnumerator)(new MMDeviceEnumerator());
                deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, out speakers);

                Guid IID_IAudioEndpointVolume = typeof(IAudioEndpointVolume).GUID;

                speakers.Activate(ref IID_IAudioEndpointVolume, 0, IntPtr.Zero, out object endpointVolume);
                IAudioEndpointVolume masterVol = (IAudioEndpointVolume)endpointVolume;

                return masterVol;
            }
            finally
            {
                if (speakers != null) Marshal.ReleaseComObject(speakers);
                if (deviceEnumerator != null) Marshal.ReleaseComObject(deviceEnumerator);
            }
        }

        #endregion

    }
}

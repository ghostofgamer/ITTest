using SOContent;
using UnityEngine;

namespace AudioContent
{
    public static class AudioPlayer
    {
        private static AudioSource _audioSource;
        private static AudioConfig _audioConfig;

        public static void Init(AudioConfig audioConfig, AudioSource audioSource)
        {
            _audioSource = audioSource;
            _audioConfig = audioConfig;
        }

        public static void PlayClickSound()
        {
            _audioSource.PlayOneShot(_audioConfig.ClickSound);
        }
    }
}
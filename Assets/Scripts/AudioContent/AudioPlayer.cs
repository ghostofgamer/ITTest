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

        public static void PlayClickSound() => _audioSource.PlayOneShot(_audioConfig.ClickSound);
        
        public static void PlayMergeSound() => _audioSource.PlayOneShot(_audioConfig.MergeSound);
        
        public static void PlayFlyItemSound() => _audioSource.PlayOneShot(_audioConfig.FlyItemSound);
        
        public static void PlayCellSpawnSound() => _audioSource.PlayOneShot(_audioConfig.CellSpawnSound);
    }
}
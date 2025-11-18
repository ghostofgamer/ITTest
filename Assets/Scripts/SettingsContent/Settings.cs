using AudioContent;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace SettingsContent
{
    public class Settings : MonoBehaviour
    {
        private const string Sound = "Sound";
        private const string SFX = "SFX";
        private const string Master = "Master";

        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private Slider _slider;
        [SerializeField] private Toggle _toggleSound;
        [SerializeField] private Toggle _toggleSFX;
        [SerializeField] private float _defaultVolume = 0.5f;

        private void Start()
        {
            _slider.onValueChanged.AddListener(SetMasterVolume);
            _slider.value = _defaultVolume;
            _toggleSound.onValueChanged.AddListener(SetSoundEnabled);
            _toggleSFX.onValueChanged.AddListener(SetSFXEnabled);
        }

        private void SetSoundEnabled(bool on)
        {
            _audioMixer.SetFloat(Sound, on ? 0 : -80);
            AudioPlayer.PlayClickSound();
        }

        private void SetSFXEnabled(bool on)
        {
            _audioMixer.SetFloat(SFX, on ? 0 : -80);
            AudioPlayer.PlayClickSound();
        }

        private void SetMasterVolume(float value)
        {
            float dB;

            if (value <= 0.0001f)
                dB = -80f;
            else
                dB = Mathf.Log10(value) * 20f;

            _audioMixer.SetFloat(Master, dB);
        }
    }
}
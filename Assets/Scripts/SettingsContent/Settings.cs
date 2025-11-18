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

// @formatter:off
        [Header("References")]
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private Slider _slider;
        [SerializeField] private Toggle _toggleSound;
        [SerializeField] private Toggle _toggleSFX;
        
        [Header("Volume Parameters")]
        [SerializeField] private float _defaultVolume = 0.5f;
// @formatter:on

        private float _onValue = 0;
        private float _offValue = -80;
        private float _factor = 20f;

        private void Start()
        {
            _slider.onValueChanged.AddListener(SetMasterVolume);
            _toggleSound.onValueChanged.AddListener(SetSoundEnabled);
            _toggleSFX.onValueChanged.AddListener(SetSFXEnabled);
            _slider.value = _defaultVolume;
            SetMasterVolume(_slider.value);
        }

        private void SetSoundEnabled(bool on)
        {
            _audioMixer.SetFloat(Sound, on ? _onValue : _offValue);
            AudioPlayer.PlayClickSound();
        }

        private void SetSFXEnabled(bool on)
        {
            _audioMixer.SetFloat(SFX, on ? _onValue : _offValue);
            AudioPlayer.PlayClickSound();
        }

        private void SetMasterVolume(float value)
        {
            float dB;

            if (value <= 0.0001f)
                dB = _offValue;
            else
                dB = Mathf.Log10(value) * _factor;

            _audioMixer.SetFloat(Master, dB);
        }
    }
}
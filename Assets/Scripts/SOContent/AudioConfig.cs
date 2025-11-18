using UnityEngine;

namespace SOContent
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "ScriptableObjects/AudioConfig", order = 1)]
    public class AudioConfig : ScriptableObject
    {
        [SerializeField] private AudioClip _clickSound;

        public AudioClip ClickSound => _clickSound;
    }
}
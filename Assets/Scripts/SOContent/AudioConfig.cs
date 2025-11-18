using UnityEngine;
using UnityEngine.Serialization;

namespace SOContent
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "ScriptableObjects/AudioConfig", order = 1)]
    public class AudioConfig : ScriptableObject
    {
        [SerializeField] private AudioClip _clickSound;
        [SerializeField] private AudioClip _mergeSound;
        [SerializeField] private AudioClip _flyItemSound;
        [SerializeField] private AudioClip _cellSpawnSound;

        public AudioClip ClickSound => _clickSound;

        public AudioClip MergeSound => _mergeSound;

        public AudioClip FlyItemSound => _flyItemSound;
        
        public AudioClip CellSpawnSound => _cellSpawnSound;
    }
}
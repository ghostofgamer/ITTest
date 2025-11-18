using System.Collections;
using AudioContent;
using SOContent;
using UnityEngine;

namespace Initialization
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private ItemSpawner _itemSpawner;
        [SerializeField] private GridCreator _gridCreator;
        [SerializeField] private AudioConfig _audioConfig;
        [SerializeField]private AudioSource _audioSource;

        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.3f);

        private void Awake()
        {
            Initialization();
        }

        private void Initialization()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StartInit());
        }

        private IEnumerator StartInit()
        {
            AudioPlayer.Init(_audioConfig,_audioSource);
            _itemSpawner.Init();
            yield return _waitForSeconds;
            _gridCreator.CreateGrid();
        }
    }
}
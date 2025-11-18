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
        [SerializeField] private LoadingScreen _loadingScreen;

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
            _loadingScreen.Show();
            yield return _waitForSeconds;
            AudioPlayer.Init(_audioConfig,_audioSource);
            _loadingScreen.SetProgress(0.3f);
            yield return _waitForSeconds;
            _itemSpawner.Init();
            _loadingScreen.SetProgress(0.6f);
            yield return _waitForSeconds;
            _loadingScreen.SetProgress(1f);
            yield return _loadingScreen.FadeOut();
            _gridCreator.CreateGrid();
        }
    }
}
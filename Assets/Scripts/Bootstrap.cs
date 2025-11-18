using System.Collections;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField]private ItemSpawner _itemSpawner;
    [SerializeField] private GridCreator _gridCreator;

    private Coroutine _coroutine;
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.3f);
    
    private void Awake()
    {
        Initialization();
    }

    private void Initialization()
    {
      if(_coroutine!=null)
          StopCoroutine(_coroutine);

      _coroutine = StartCoroutine(StartInit());
    }

    private IEnumerator StartInit()
    {
        _itemSpawner.Init();
        yield return _waitForSeconds;
        _gridCreator.CreateGrid();
    }
}
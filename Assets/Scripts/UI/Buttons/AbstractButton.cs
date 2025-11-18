using AudioContent;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public abstract class AbstractButton : MonoBehaviour
    {
        [SerializeField] private bool _playSound = true;
        
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClickInternal);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClickInternal);
        }
        
        private void OnClickInternal()
        {
            if (_playSound)
                AudioPlayer.PlayClickSound();

            Click();
        }

        protected abstract void Click();
    }
}
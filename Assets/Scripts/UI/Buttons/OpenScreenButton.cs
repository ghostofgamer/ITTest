using UI.Screens;
using UnityEngine;

namespace UI.Buttons
{
    public class OpenScreenButton : AbstractButton
    {
        [SerializeField] private AbstractScreen _screen;

        protected override void Click() => _screen.OpenScreen();
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    [SerializeField] private Button _buttonToMenu;
    [SerializeField] private StartMenu _menuStart;
    [Space]
    [SerializeField] private GameArea _gameArea;

    public bool ControlEnableGame
    {
        set
        {
            _gameArea.ControlEnable = value;
            _buttonToMenu.interactable = value;
        }
    }
    public bool ControlEnableUI { set => _menuStart.ControlEnable = value; }
    public bool ControlEnable { set => ControlEnableGame = ControlEnableUI = value; }

    private event Action EventPrivateContinue;

    public event Action EventPause;
    public event Action EventContinueGame { add { _menuStart.EventContinue += value; EventPrivateContinue += value; } remove { _menuStart.EventContinue -= value; EventPrivateContinue -= value; } }
    public event Action EventNewGame { add { _menuStart.EventNew += value; } remove { _menuStart.EventNew -= value; } }
    public event Action EventResetGame { add { _menuStart.EventReset += value; } remove { _menuStart.EventReset -= value; } }

    private void Awake()
    {
        _buttonToMenu.onClick.AddListener(OnButtonClick);

        #region Local Function
        void OnButtonClick()
        {
            if(_gameArea.IsOpen)
                EventPause?.Invoke();
            else
                EventPrivateContinue?.Invoke();
        }
        #endregion
    }
}

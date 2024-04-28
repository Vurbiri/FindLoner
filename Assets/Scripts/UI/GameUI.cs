using System;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Button _menuButton;
    [Space]
    [SerializeField] private MenuNavigation _mainMenu;
    [Space]
    [SerializeField] private MenuGroup _settings;
    [SerializeField] private MenuGroup _leaderboard;
    [Space]
    [SerializeField] private GameObject _shading;
    [Space]
    [SerializeField] private Vector3 _positionOff = new(-1000, -1000, 0);

    private bool _isPause = false, _isStartNewGame = false;
    private Vector3 _thisPosition;
    private Transform _thisTransform;
    private LeaderboardUI _leaderboardUI;

    public bool ControlEnable { get; set; }

    public event Action EventPause;
    public event Action EventPlay;
    public event Action EventStart;

    private void Awake()
    {
        _thisTransform = transform;
        _thisPosition = _thisTransform.position;

        _leaderboardUI = _leaderboard.GetComponent<LeaderboardUI>();
    }

    private void Start()
    {
        _menuButton.onClick.AddListener(OnMenu);
        Off();
    }

    public void Open()
    {
        EventPause?.Invoke();
        _isPause = true;

        On();
        _settings.Enable = true;
        _leaderboard.Enable = false;
    }
    public void OpenLeaderboard()
    {
        EventPause?.Invoke();
        _isPause = _isStartNewGame = true;

        On();
        _settings.Enable = false;
        _leaderboard.Enable = true;
    }
    public void Close()
    {
        _isPause = false;
        Off();
        EventPlay?.Invoke();

        if (_isStartNewGame)
        {
            EventStart?.Invoke();
            _isStartNewGame = false;
        }
    }

    public void SetScore(long score, Action<bool> callback) => _leaderboardUI.SetScore(score, callback);

    private void Off()
    {
        _thisTransform.position = _positionOff;
        _mainMenu.SetButtonsActive(false);
    }
    private void On()
    {
        _mainMenu.SetButtonsActive(true);
        _shading.SetActive(true);
        _thisTransform.position = _thisPosition;
    }

    private void OnMenu()
    {
        if (!ControlEnable) return;

        if (_isPause)
            Close();
        else
            Open();
    }


    #region Nested Classe
    [System.Serializable]
    private class MenuGroup
    {
        [SerializeField] private GameObject _menu;
        [SerializeField] private Selectable _button;

        public bool Enable 
        { 
            set 
            {
                _menu.SetActive(value);

                if (value)
                    _button.Select();
            } 
        }

        public T GetComponent<T>() where T : Component => _menu.GetComponent<T>();
    }
    #endregion
}

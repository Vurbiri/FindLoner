using System;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private LeaderboardUI _leaderboardUI;
    [Space]
    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject _mainMenu;
    [Space]
    [SerializeField] private MenuGroup _settings;
    [SerializeField] private MenuGroup _leaderboard;

    private bool _isPause = false, _isStartNewGame = false;

    public bool ControlEnable { get; set; }

    public event Action EventPause;
    public event Action EventPlay;
    public event Action EventStart;

    private void Start()
    {
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
    public void OnMenu()
    {
        if (!ControlEnable) return;

        if (_isPause)
            Close();
        else
            Open();
    }

    public void SetScore(long score, Action<bool> callback) => StartCoroutine(_leaderboardUI.SetScore(score, callback));

    private void Off()
    {
        _background.SetActive(false);
        _mainMenu.SetActive(false);
        _settings.Enable = false;
        _leaderboard.Enable = false;
    }

    private void On()
    {
        _background.SetActive(true);
        _mainMenu.SetActive(true);
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
    }
    #endregion
}

using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private AnimationController _menus;
    [Space]
    [SerializeField] private LeaderboardUI _leaderboardUI;
    [Space]
    [SerializeField] private MenuGroup _start;
    [SerializeField] private MenuGroup _settings;
    [SerializeField] private MenuGroup _leaderboard;
    [SerializeField] private MenuGroup _help;

    private void Start()
    {
        EnableMenus(SettingsGame.Instance.IsFirstStart);
    }

    public void Open()
    {
        EnableMenus();
        _menus.PlayNormal();
    }
    public void Close()
    {
        _menus.PlayRevers();

        _start.Enable = false;
        _settings.Enable = false;
        _leaderboard.Enable = false;
        _help.Enable = false;
    }
    public void OpenLeaderboard()
    {
        _start.Enable = false;
        _settings.Enable = false;
        _leaderboard.Enable = true;
        _help.Enable = false;

        _menus.PlayNormal();
    }

    public void SetScore(long score) => _leaderboardUI.SetScore(score).Forget();

    private void EnableMenus(bool isFirst = false)
    {
        _start.Enable = !isFirst;
        _settings.Enable = false;
        _leaderboard.Enable = false;
        _help.Enable = isFirst;
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

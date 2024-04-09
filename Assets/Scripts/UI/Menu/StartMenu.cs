using System;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MenuNavigation
{
    private DataGame _dataGame;

    public event Action EventContinue;
    public event Action EventNew;
    public event Action EventReset;

    protected override void Awake()
    {
        base.Awake();
        _dataGame = DataGame.Instance;
    }

    public void OnContinue()
    {
        EventContinue?.Invoke();
    }

    public void OnNew()
    {
        if (_dataGame.IsNewGame)
        {
            EventNew?.Invoke();
            return;
        }

        EventReset?.Invoke();
    }
}

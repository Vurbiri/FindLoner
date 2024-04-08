using System;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MenuNavigation
{
    [Space]
    [SerializeField] private ToggleFullInteractable _toggleContinue;
    [SerializeField] private ToggleFullInteractable _toggleNew;
    [SerializeField] private SliderFullInteractable _sliderMax;
    [Space]
    [SerializeField] private Button _startOk;

    private DataGame _dataGame;
    private float _tempValueMax;

    public bool ControlEnable { set => _startOk.interactable = value; }

    public event Action EventContinue;
    public event Action EventNew;
    public event Action EventReset;

    protected override void Awake()
    {
        base.Awake();

        _dataGame = DataGame.Instance;

        _toggleContinue.Initialize();
        _toggleNew.Initialize();
        _sliderMax.Initialize(_dataGame.MinDigit);

        _toggleContinue.OnValueChanged.AddListener(ChangeGameStart);
        _sliderMax.Value = _tempValueMax = _dataGame.MaxDigit;
    }

    private void OnEnable()
    {
        bool isContinueGame = !_dataGame.IsNewGame;

        _toggleContinue.Interactable = isContinueGame;
        
        _toggleContinue.IsOn = isContinueGame;
        _toggleNew.IsOn = !isContinueGame;

        _sliderMax.Interactable = !isContinueGame;
    }

    private void ChangeGameStart(bool isContinueGame)
    {
        if (isContinueGame)
        {
            _tempValueMax = _sliderMax.Value;
            _sliderMax.Value = _dataGame.MaxDigit;
            _sliderMax.Interactable = false;
        }
        else
        {
            _sliderMax.Value = _tempValueMax;
            _sliderMax.Interactable = true;
        }
    }

    public void OnStart()
    {
        if (_toggleContinue.IsOn)
        {
            EventContinue?.Invoke();
            return;
        }

        _dataGame.MaxDigit = Mathf.RoundToInt(_sliderMax.Value);

        if (_dataGame.IsNewGame)
        {
            EventNew?.Invoke();
            return;
        }

        EventReset?.Invoke();
        return;
    }
}

using UnityEngine;

public class ProgressBars : MonoBehaviour
{
    [SerializeField] private BonusLevels _bonusLevels;
    [SerializeField] private Timer _timerGameLevel;
    [SerializeField] private Timer _timerBonusLevel;
    [Space]
    [SerializeField] private ProgressBar _progressBarRight;
    [SerializeField] private ProgressBar _progressBarLeft;

    private void Start()
    {
        _timerGameLevel.EventSetTime += SetMaxValue;
        _timerGameLevel.EventTick += SetValue;
        _timerGameLevel.EventEndTime += Clear;
        _timerGameLevel.EventStop += Clear;

        _timerBonusLevel.EventSetTime += SetMaxValue;
        _timerBonusLevel.EventTick += SetValue;

        _bonusLevels.EventSetMaxAttempts += (v) => SetMaxValue(v);
        _bonusLevels.EventChangedAttempts += SetSmoothValue;
        //_bonusLevels.EventEndLevel += _ => Clear();

        #region Local functions
        //===========================================================================================
        void SetMaxValue(float value) => _progressBarRight.MaxValue = _progressBarLeft.MaxValue = value;
        void SetValue(float value) => _progressBarRight.Value = _progressBarLeft.Value = value;
        void SetSmoothValue(int value) => _progressBarRight.SmoothValue = _progressBarLeft.SmoothValue = value;
        void Clear() { _progressBarRight.ClearSmooth(); _progressBarLeft.ClearSmooth(); }
        #endregion
    }
}

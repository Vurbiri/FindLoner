using UnityEngine;

public class ProgressBars : MonoBehaviour
{
    [SerializeField] private BonusLevels _bonusLevels;
    [SerializeField] private Timer _timer;
    [Space]
    [SerializeField] private ProgressBar _progressBarRight;
    [SerializeField] private ProgressBar _progressBarLeft;

    private void Awake()
    {
        _timer.EventSetTime += SetMaxValue;
        _timer.EventTick += SetValue;
        _timer.EventStop += Clear;

        _bonusLevels.EventSetMaxAttempts += (v) => SetMaxValue(v);
        _bonusLevels.EventChangedAttempts += SetSmoothValue;
        //_bonusLevels.EventEndLevel += _ => Clear();

        #region Local functions
        //===========================================================================================
        void SetMaxValue(float value) => _progressBarRight.MaxValue = _progressBarLeft.MaxValue = value;
        void SetValue(float value) => _progressBarRight.Value = _progressBarLeft.Value = value;
        void SetSmoothValue(int value) => _progressBarRight.SmoothValue = _progressBarLeft.SmoothValue = value;
        void Clear() => SetSmoothValue(0);
        #endregion
    }
}

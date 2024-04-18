using UnityEngine;

public class ProgressBars : MonoBehaviour
{
    [SerializeField] private BonusLevels _bonusLevels;
    [Space]
    [SerializeField] private ProgressBar _progressBarRight;
    [SerializeField] private ProgressBar _progressBarLeft;

    private void Awake()
    {
        _bonusLevels.EventSetMaxAttempts += (v) => SetMaxValue(v);
        _bonusLevels.EventChangedAttempts += SetSmoothValue;
        _bonusLevels.EventEndLevel += Clear;

        #region Local functions
        //===========================================================================================
        void SetMaxValue(float value) => _progressBarRight.MaxValue = _progressBarLeft.MaxValue = value;
        void SetSmoothValue(int value) => _progressBarRight.SmoothValue = _progressBarLeft.SmoothValue = value;
        #endregion
    }

    //private void Start()
    //{
    //    Clear();
    //}

    private void Clear() => _progressBarRight.Value = _progressBarLeft.Value = 0;
}

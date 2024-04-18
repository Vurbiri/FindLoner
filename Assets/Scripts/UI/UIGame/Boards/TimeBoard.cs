using TMPro;
using UnityEngine;

public class TimeBoard : MonoBehaviour
{
    [SerializeField] private BonusLevels _bonusLevels;
    [Space]
    [SerializeField] private TMP_Text _time;

    private void Awake()
    {
        _time.text = "0";
        _bonusLevels.EventSetTime += SetTime;
        _bonusLevels.EventAddTime += SetTime;
        //_bonusLevels.EventEndLevel += () => _time.text = "0";

        // Local function
        void SetTime(int value)
        {
            value /= 1000;
            _time.text = $"{value / 60} : {value % 60:D2}";
        }
    }
}

using TMPro;
using UnityEngine;

public class AttemptsBoard : MonoBehaviour
{
    [SerializeField] private BonusLevels _bonusLevels;
    [Space]
    [SerializeField] private TMP_Text _attempts;

    private void Awake()
    {
        _attempts.text = "0";
        _bonusLevels.EventChangedAttempts += (v) => _attempts.text = v.ToString();
        _bonusLevels.EventEndLevel += () => _attempts.text = "0";
    }
}

using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class CardTimeText : MonoBehaviour
{
    [SerializeField] private float _scaleFontSize = 5.2f;

    private TMP_Text _thisText;
    private Transform _thisTransform;
    private RectTransform _thisRectTransform;

    private void Awake()
    {
        _thisText = GetComponent<TMP_Text>();
        _thisRectTransform = GetComponent<RectTransform>();
        _thisTransform = transform;
    }

    public void SetSize(Vector2 size)
    {
        _thisRectTransform.sizeDelta = size;
        _thisText.fontSize = size.x * _scaleFontSize;
    }

    public void Setup(BonusTime bonus)
    {
        if (bonus == null)
        {
            _thisText.text = "";
            return;
        }
        ReSetup(bonus);
    }

    public void ReSetup(BonusTime bonus)
    {
        _thisText.color = bonus.Color;
        _thisText.text = bonus.Value.ToString();
    }

    public void SetActive(bool active) => gameObject.SetActive(active);

    public void ResetAngle() => _thisTransform.localRotation = Quaternion.identity;
    public void Rotation(Vector3 axis, float angle) => _thisTransform.rotation *= Quaternion.Euler(axis * angle);
}

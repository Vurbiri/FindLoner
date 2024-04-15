using TMPro;
using UnityEngine;


[RequireComponent(typeof(TMP_Text))]
public class CardTimeText : MonoBehaviour
{
    protected TMP_Text _thisText;
    protected Transform _thisTransform;

    private void Awake()
    {
        _thisText = GetComponent<TMP_Text>();
        _thisTransform = transform;
    }

    public void Setup(float size, BonusTime bonus)
    {
        _thisText.fontSize = size;
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

    public void Clear() => _thisText.text = "";

    public void SetActive(bool active) => gameObject.SetActive(active);

    public void ResetAngle() => _thisTransform.localRotation = Quaternion.identity;
    public void Rotation(Vector3 axis, float angle) => _thisTransform.rotation *= Quaternion.Euler(axis * angle);
}

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

    public void Setup(float size, int value)
    {
        _thisText.fontSize = size;
        _thisText.text = value < 0 ? string.Empty : value.ToString();
    }

    public void SetColor(Color color) => _thisText.color = color;
    public void SetActive(bool active) => gameObject.SetActive(active);

    public void Rotation(Vector3 axis, float angle)
    {
        _thisTransform.rotation *= Quaternion.Euler(axis * angle);
    }
}

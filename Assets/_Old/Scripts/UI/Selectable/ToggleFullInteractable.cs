using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleFullInteractable : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;
    [SerializeField] private Graphic _background;
    [SerializeField] private Color _textColor = Color.white;
    [SerializeField, Range(0f, 1f)] float _checkmarkAlfaOff = 0.25f;

    private Toggle _thisToggle;
    private Graphic _checkmark;
    private ColorBlock _colorBlock;

    public bool Interactable
    {
        get => _thisToggle.interactable;
        set
        {
            _thisToggle.interactable = value;
            SetColor();
        }
    }
    public virtual bool IsOn { get => _thisToggle.isOn; set => _thisToggle.isOn = value; }
    public UnityEvent<bool> OnValueChanged => _thisToggle.onValueChanged;

    public void Initialize()
    {
        _thisToggle = GetComponent<Toggle>();
        _colorBlock = _thisToggle.colors;

        _checkmark = _thisToggle.graphic;
        _thisToggle.graphic = null;

        _thisToggle.onValueChanged.AddListener(SetAlfaCheckmark);
        SetColor();
    }

    private void SetColor()
    {
        Color color = _thisToggle.interactable ? _colorBlock.normalColor : _colorBlock.disabledColor;
        _background.CrossFadeColor(color, _colorBlock.fadeDuration, true, true);
        _label.color = _textColor * color;

        SetAlfaCheckmark(_thisToggle.isOn);
    }

    private void SetAlfaCheckmark(bool isOn)
    {
        _checkmark.CrossFadeAlpha(Interactable && isOn ? 1f : (isOn ? _checkmarkAlfaOff : 0f) , _colorBlock.fadeDuration, true);
    }
}

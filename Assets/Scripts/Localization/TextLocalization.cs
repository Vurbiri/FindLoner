using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TextLocalization : MonoBehaviour
{
    public TMP_Text Text {get; protected set;}
    protected string _keyString;    

    public void Setup(string keyString)
    {
        Text = GetComponent<TMP_Text>();
        _keyString = string.IsNullOrEmpty(keyString) ? Text.text : keyString;
        SetText();
        Localization.Instance.EventSwitchLanguage += SetText;
    }
    private void OnDestroy()
    {
        if(Localization.Instance != null)
            Localization.Instance.EventSwitchLanguage -= SetText;
    }

    protected virtual void SetText() => Text.text = Localization.Instance.GetText(_keyString);
}

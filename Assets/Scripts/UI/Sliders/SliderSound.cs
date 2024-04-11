using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderSound : MonoBehaviour
{
    [SerializeField] private MixerGroup _audioMixerGroup;
    [SerializeField] private TextLocalization _captionText;

    protected Slider _thisSlider;
    protected SettingsGame _settings;

    private void Awake()
    {
        _thisSlider = GetComponent<Slider>();
        _settings = SettingsGame.InstanceF;

        _thisSlider.minValue = _settings.MinValue;
        _thisSlider.maxValue = _settings.MaxValue;

        _captionText.Setup(_audioMixerGroup.ToString());
    }

    private void OnEnable()
    {
        _thisSlider.value = _settings.GetVolume(_audioMixerGroup);
        _thisSlider.onValueChanged.AddListener((v) => _settings.SetVolume(_audioMixerGroup, v));
    }

    private void OnDisable()
    {
        _thisSlider.onValueChanged.RemoveAllListeners();
    }
}

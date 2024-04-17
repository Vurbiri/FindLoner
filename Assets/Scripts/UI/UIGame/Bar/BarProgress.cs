using UnityEngine;
using UnityEngine.UI;

public class BarProgress : MonoBehaviour
{
    [SerializeField] private Slider _sliderTop;
    [SerializeField] private Slider _sliderBottom;

    private Graphic _imageFillTop;
    private Graphic _imageFillBottom;

    public float Value { get => _sliderTop.value; set => _sliderTop.value = _sliderBottom.value = value; }

    public float MaxValue { set => _sliderTop.maxValue = _sliderBottom.maxValue = value; }

    public Color Color { set => _imageFillTop.color = _imageFillBottom.color = value; }


    private void Awake()
    {
        _imageFillTop = _sliderTop.targetGraphic;
        _imageFillBottom = _sliderBottom.targetGraphic;
    }
}

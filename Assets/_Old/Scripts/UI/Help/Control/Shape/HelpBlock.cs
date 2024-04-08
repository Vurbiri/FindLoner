using UnityEngine;
using UnityEngine.UI;

public class HelpBlock : MonoBehaviour
{
    [SerializeField] private Image _imageBlock;
    [SerializeField] private Image _imageNumber;

    private Color _colorBlock;
    private Color _colorNumber;

    public void Initialize()
    {
        _colorBlock = _imageBlock.color;
        _colorNumber = _imageNumber.color;
    }

    public void SetColorAlfa(float alfa)
    {
        _colorBlock.a = alfa;
        _colorNumber.a = alfa;

        _imageBlock.color = _colorBlock;
        _imageNumber.color = _colorNumber;
    }
}

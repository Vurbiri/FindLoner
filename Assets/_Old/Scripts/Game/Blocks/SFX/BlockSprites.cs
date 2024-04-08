using UnityEngine;

public class BlockSprites : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRendererBlock;
    [SerializeField] private SpriteRenderer _spriteRendererNumber;
    [Space]
    [SerializeField] private int _baseOrderInLayer = 0;
    [SerializeField] private int _moveOrderInLayer = 2;

    private Transform _thisTransform;
    private Vector3 _defaultPosition;

    private Color _colorBlock;
    private Color _colorNumber;

    private void Awake()
    {
        _thisTransform = transform;
        _defaultPosition = _thisTransform.localPosition;
    }

    public void Setup(BlockSettings settings)
    {
        _spriteRendererBlock.sprite = settings.SpriteBlock;
        _spriteRendererBlock.color = _colorBlock = settings.ColorBlock;

        _spriteRendererNumber.sprite = settings.SpriteNumber;
        _spriteRendererNumber.color = _colorNumber = settings.ColorNumber;

        gameObject.SetActive(true);
    }

    public void StartMove()
    {
        SetOrderLayer(_moveOrderInLayer);
    }
    public void StopMove()
    {
        SetOrderLayer(_baseOrderInLayer);
    }

    public void ProjectionOn(Vector3 position, float alfaProjecting)
    {
        _thisTransform.position = position;
        SetColorAlfa(alfaProjecting);
    }

    public void ProjectionOff()
    {
        _thisTransform.localPosition = _defaultPosition;
        SetColorAlfa(1f);
    }

    public void Off()
    {
        gameObject.SetActive(false);
    }

    private void SetColorAlfa(float alfa)
    {
        _colorBlock.a = alfa;
        _colorNumber.a = alfa;

        _spriteRendererBlock.color = _colorBlock;
        _spriteRendererNumber.color = _colorNumber;
    }

    private void SetOrderLayer(int layer)
    {
        _spriteRendererBlock.sortingOrder = layer;
        _spriteRendererNumber.sortingOrder = ++layer;
    }
}

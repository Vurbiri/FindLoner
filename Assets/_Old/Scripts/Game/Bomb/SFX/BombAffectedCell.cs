using UnityEngine;

public class BombAffectedCell : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRendererCell;

    public void Initialize(Vector3 position)
    {
        transform.localPosition = position;
        gameObject.SetActive(false);
    }

    public void ProjectionOn()
    {
        gameObject.SetActive(true);
    }

    public void ProjectionOff()
    {
        gameObject.SetActive(false);
    }

    public void SetColor(Color color) => _spriteRendererCell.color = color;
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class CardsManager : MonoBehaviour
{
    [SerializeField] private Card _prefabCard;
    [Space]
    [SerializeField] private List<Sprite> _shapeSprites;
    [Space]
    [SerializeField] private float _speedRotation = 90f;

    private GridLayoutGroup _thisGrid;
    private Transform _thisTransform;
    private float _size;

    private List<Card> _cards = new();

    private const float SATURATION_MIN = 0.16f;
    private const float BRIGHTNESS_MIN = 0.2f;

    private void Awake()
    {
        _thisGrid = GetComponent<GridLayoutGroup>();
        _thisTransform = transform;
        _size = GetComponent<RectTransform>().rect.size.x;
    }

    private void Start()
    {
        Test();
    }


    private void Test()
    {
        int side = 12, count = side * side;
        Card card;

        _thisGrid.constraintCount = side;
        _thisGrid.cellSize = Vector2.one * _size / side;
        List<Shape> shapes = GetShapes(8, true);
        int index;

        for (int i = 0; i < count; i++) 
        {
            index = Random.Range(0, shapes.Count);

            card = Instantiate(_prefabCard, _thisTransform);
            card.Setup(shapes[index], side, Vector3.up);

        }
    }

    private List<Shape> GetShapes(int count, bool monochrome) 
    {
        List<Shape> shapes = new(count);
        Shape shape;
        int index;
        Color color = Color.white; color.Randomize(SATURATION_MIN, BRIGHTNESS_MIN);

        for (int i = 0; i < count; i++)
        {
            index = Random.Range(0, _shapeSprites.Count);

            shape = new(_shapeSprites[index], color);
            if (!monochrome)
                shape.SetUniqueColor(shapes, SATURATION_MIN, BRIGHTNESS_MIN);
            shapes.Add(shape);

            _shapeSprites.RemoveAt(index);
        }

        foreach (var s in shapes)
            _shapeSprites.Add(s.Sprite);

        return shapes;
    }

}

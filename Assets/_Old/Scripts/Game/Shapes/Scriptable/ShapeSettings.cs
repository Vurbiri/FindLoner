using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShape", menuName = "DBlocks/Shape", order = 51)]
public class ShapeSettings : ScriptableObject, IRandomizeObject
{
    [SerializeField] private ShapeType _type;
    [SerializeField] private Vector2Int _size;
    [SerializeField] private Vector2Int[] _startBlocksPositions;
    [Header("Random")]
    [SerializeField] private int _randomWeight = 100;

    public ShapeType Type => _type;
    public Vector3 Size { get; private set; }
    public Vector2Int[] BlocksPositions => _startBlocksPositions;
    public Vector2Int[] Indexes { get; private set; }
    public Vector2 Offset { get; private set; }

    [ShowNativeProperty]
    public int Weight => _randomWeight + BonusWeight;
    [ShowNativeProperty]
    public int BonusWeight { get; set; }
    public int MaxCount => 1;

    public void Initialize(int bonus = 0)
    {
        BonusWeight = bonus;

        Size = _size.ToVector3();
        Offset = (Vector3.one - Size) / 2f;

        Indexes = new Vector2Int[_startBlocksPositions.Length];
        Indexes[0] = Vector2Int.zero;
        Vector2Int first = _startBlocksPositions[0];
        for (int i = 1; i < _startBlocksPositions.Length; i++)
            Indexes[i] = _startBlocksPositions[i] - first;
    }
}

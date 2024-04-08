using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolBlocks : ASingleton<PoolBlocks>
{
    [Space]
    [SerializeField] private int _size = 100;
    [SerializeField] private Block _prefabBlock;
    [SerializeField] private BlockSettings[] _settingsBlocks;
    [Space]
    [SerializeField] private int _baseWeight = 100;
    [SerializeField] private int _baseWeightForAdd = 4;

    private Pool<Block> _poolBlocks;
    private ArrayRandom<BlockSettings> _randomSettings;

    private BlockSettings _blockOne;
       

    protected override void Awake()
    {
        base.Awake();

        Array.Sort(_settingsBlocks, (a, b) => a.Digit.CompareTo(b.Digit));
        _blockOne = _settingsBlocks[0];
        for(int i = 1; i < _settingsBlocks.Length; i++)
            _settingsBlocks[i].Weight = _baseWeight + _baseWeightForAdd * (i - 1);
        _randomSettings = new(_settingsBlocks);
        _settingsBlocks = null;

        Initialize();
    }

    public void Initialize()
    {
        _poolBlocks = new(_prefabBlock, transform, _size);
    }

    public void Setup(int maxDigit)
    {
        _blockOne.Weight = _baseWeight + _baseWeightForAdd * (maxDigit - 1);
        _randomSettings.ReInitialize(maxDigit);
    }

    public Block GetBlock(Transform parent, Vector3 position, int digit)
    {
        Block block = _poolBlocks.GetObject(parent);
        block.Setup(position, _randomSettings[digit - 1]);
        return block;
    }

    public List<Block> GetBlocks(Transform parent, Vector2Int[] positions, Vector2 offset)
    {
        int count = positions.Length;
        _blockOne.MaxCount = count - 1;
        List<Block> blocks = _poolBlocks.GetObjects(parent, count);
        BlockSettings[] blockSettings = _randomSettings.Range(count);

        for (int i = 0; i < count; i++)
            blocks[i].Setup(offset + positions[i], blockSettings[i]);

        return blocks;
    }

    public List<Block> GetBlocks(Transform parent, int[] digits, Vector2Int[] positions, Vector2 offset)
    {
        int count = positions.Length;
        _blockOne.MaxCount = count - 1;
        List<Block> blocks = _poolBlocks.GetObjects(parent, count);

        for (int i = 0; i < count; i++)
            blocks[i].Setup(offset + positions[i], _randomSettings[digits[i] - 1]);

        return blocks;
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_settingsBlocks == null || _settingsBlocks.Length == 0)
            return;
        
        Array.Sort(_settingsBlocks, (a, b) => a.Digit.CompareTo(b.Digit));
    }
#endif
}

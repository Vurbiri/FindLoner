using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksArea : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [Space]
    [SerializeField] private float _timePauseBlocksRemoved = 0.225f;
    
    public const int size = 10;
    private const int MAX_CELL = size * size;

    public bool Blocking { get; set; }
    public bool IsFill => _countFill >= MAX_CELL;

    private Block this[Vector2Int index]
    {
        get => _blocks[index.x, index.y];
        set
        {
            _blocks[index.x, index.y] = value;
            _saveArea[index.x, index.y] = value == null ? 0 : value.Digit;
        }
    }

    private DataGame _dataGame;
    private PoolBlocks _poolBlocks;
    
    private Rect _bounds;
    private Block[,] _blocks;
    private int[,] _saveArea;

    private int _countFill = 0;

    private WaitForSecondsRealtime _pauseBlocksRemoved;

    private void Awake()
    {
        _dataGame = DataGame.Instance;
        _poolBlocks = PoolBlocks.Instance;
        
        _blocks = new Block[size, size];
        _saveArea = _dataGame.SaveArea;
        _bounds = new(Vector2.zero, new(size, size));
        
        _pauseBlocksRemoved = new(_timePauseBlocksRemoved);
    }

    public void Load()
    {
        int digit;
        Block block;
        Vector2Int index = Vector2Int.zero;

        for (int x = 0; x < size; x++) 
        { 
            for (int y = 0; y < size; y++) 
            {
                if ((digit = _saveArea[x, y]) == 0)
                    continue;
                
                index.x = x; index.y = y;
                block = _poolBlocks.GetBlock(_container, index.ToVector3(), digit);
                block.Index = index;
                Add(block);
            }
        }
    }

    #region Checking
    public bool IsCorrectIndex(Vector2Int index) => index.x >= 0 && index.x < size && index.y >= 0 && index.y < size;
    public bool IsFilledIndex(Vector2Int index) => this[index] != null;
    private bool IsEmptyIndex(Vector2Int index) => IsCorrectIndex(index) && this[index] == null;

    public bool IsEmptyCells(List<Block> blocks)
    {
        foreach (var block in blocks)
        {
            if (!IsEmptyCell(block))
                return false;
        }
        return true;
    }
    public bool IsContains(List<Block> blocks)
    {
        foreach (var block in blocks)
        {
            if (!_bounds.Contains(block.Position))
                return false;
        }
        return true;
    }
    public bool IsEmptyCell(IBlock block) => !Blocking && _bounds.Contains(block.Position) && this[block.GetIndex()] == null;

    public bool IsInsert(Vector2Int[] indexes)
    {
        if(_countFill + indexes.Length > MAX_CELL)
            return false;

        Vector2Int index = Vector2Int.zero;
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (_blocks[x, y] != null)
                    continue;

                index.x = x; index.y = y;
                if (CheckIndexes()) 
                    return true;
            }
        }

        return false;

        #region Local Functions
        bool CheckIndexes()
        {
            for (int i = 1; i < indexes.Length; i++)
                if(!IsEmptyIndex(index + indexes[i]))
                    return false;

            return true;
        }
        #endregion
    }
    #endregion

    public void AddBlocks(List<Block> blocks)
    {
        HashSet<Block> blocksSeries = new();
        HashSet<Block> blocksOne = new();
        HashSet<Block> blocksRemove = new();

        blocks.ForEach(block => Fixed(block));
        blocks.Sort((a, b) => b.Digit.CompareTo(a.Digit));

        int score = 0;
        foreach (var block in blocks)
        {
            score += block.Digit;

            if (block.IsOne)
                continue;

            if (CreateSeries(block))
            {
                _dataGame.ScoreForSeries(block.Digit, blocksSeries.Count, blocksOne.Count);
                blocksRemove.UnionWith(blocksSeries);
                blocksRemove.UnionWith(blocksOne);
            }
            blocksSeries.Clear();
            blocksOne.Clear();
        }
        _dataGame.ScoreForAdd(score);
        blocks.Clear();

        if (blocksRemove.Count == 0)
            return;

        foreach (var block in blocksRemove)
            Remove(block);

        StartCoroutine(AnimationRemoveBlocks(blocksRemove));

        #region Local Functions
        void Fixed(Block block)
        {
            block.Fixed(_container);
            Add(block);
        }
        bool CreateSeries(Block block)
        {
            if (!AddToSeries(block))
                return false;

            foreach (var d in Direction2D.Line)
                if (TryGetBlock(block.Index + d, out Block outBlock))
                    if (outBlock.IsOne || block.IsEqualDigit(outBlock))
                        CreateSeries(outBlock);

            return blocksSeries.Count >= block.Digit;

            #region Local Functions
            bool AddToSeries(Block addBlock)
            {
                if (blocksRemove.Contains(addBlock))
                    return false;

                if (addBlock.IsOne)
                    return blocksOne.Add(addBlock);
                else
                    return blocksSeries.Add(addBlock);
            }
            #endregion
        }
        IEnumerator AnimationRemoveBlocks(HashSet<Block> blocksAnimation)
        {
            foreach (var block in blocksAnimation)
            {
                StartCoroutine(block.RemoveCoroutine());
                yield return _pauseBlocksRemoved;
            }
        }
        #endregion
    }

    public void ExplodeBlocks(Vector2Int index)
    {
        List<Block> blocks = new(8);

        foreach (var d in Direction2D.All)
        {
            if (TryGetBlock(index + d, out Block outBlock))
            {
                Remove(outBlock);
                blocks.Add(outBlock);
            }
        }

        foreach (var block in blocks)
            StartCoroutine(block.ExplodeCoroutine());
    }

    public void RemoveAll()
    {
        if (_countFill <= 0)
            return;

        foreach (var block in _blocks)
        {
            if (block == null)
                continue;

            Remove(block);
            StartCoroutine(block.RemoveSilentlyCoroutine());
        }
    }

    private void Add(Block block)
    {
        this[block.Index] = block;
        _countFill++;
    }

    private void Remove(Block block)
    {
        this[block.Index] = null;
        _countFill--;
    }

    private bool TryGetBlock(Vector2Int index, out Block block)
    {
        block = null;
        if (!IsCorrectIndex(index))
            return false;

        block = this[index];
        return block != null;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        _bounds = new(Vector2.zero, new(size, size));
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_bounds.center, _bounds.size);
    }
#endif
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAffectedArea : MonoBehaviour
{
    [SerializeField] private BombAffectedCell _prefabAffectedCell;
    [Space]
    [SerializeField] private Color _color = Color.white;
    [SerializeField] private Vector2 _alfaMinMaxTick = new(0.4f, 0.72f);

    private readonly BombAffectedCell[] _cells = new BombAffectedCell[Direction2D.All.Length];
    private readonly HashSet<BombAffectedCell> _tickingCells = new(Direction2D.All.Length);
    private Vector2Int[] _directionAll;
    private BlocksArea _area;

    private Transform _thisTransform;
    private Vector3 _defaultPosition;

    private float _speedTick;
    private Coroutine _coroutineTick = null;

    public void Initialize(BlocksArea area, float timeTick)
    {
        _thisTransform = transform;
        _defaultPosition = _thisTransform.localPosition;

        _directionAll = Direction2D.All;
        _area = area;

        _speedTick = (_alfaMinMaxTick.y - _alfaMinMaxTick.x) / timeTick;

        for (int i = 0; i < _directionAll.Length; i++) 
        {
            _cells[i] = Instantiate(_prefabAffectedCell, _thisTransform);
            _cells[i].Initialize(_directionAll[i].ToVector3());
        }
    }

    public void ProjectionOn(Vector3 position, Vector2Int index, bool isShift)
    {
        _thisTransform.position = position;
        _coroutineTick ??= StartCoroutine(TickBombCoroutine());

        if (!isShift) return;

        Vector2Int around;
        BombAffectedCell cell;
        for (int i = 0; i < _directionAll.Length; i++)
        {
            cell = _cells[i];
            around = index + _directionAll[i];
            if (_area.IsCorrectIndex(around))
            {
                cell.ProjectionOn();
                if(_area.IsFilledIndex(around))
                    _tickingCells.Add(cell);
                else
                    CellTickingRemove();
            }
            else
            {
                cell.ProjectionOff();
                CellTickingRemove();
            }
        }

        #region Local Function
        void CellTickingRemove()
        {
            if (_tickingCells.Remove(cell))
                cell.SetColor(_color);
        }
        #endregion
    }

    public void ProjectionOff()
    {
        if (_coroutineTick != null)
        {
            StopCoroutine(_coroutineTick);
            _coroutineTick = null;
            foreach (var cell in _tickingCells)
                cell.SetColor(_color);

            _tickingCells.Clear();
        }
        _thisTransform.localPosition = _defaultPosition;
        foreach (var cell in _cells)
            cell.ProjectionOff();
    }

    public void Explode()
    {
        ProjectionOff();
    }

    public void ResetState()
    {
        ProjectionOff();
    }

    private IEnumerator TickBombCoroutine()
    {
        float alfa = _alfaMinMaxTick.y;
        Color color = _color;
        while (true)
        {
            color.a = alfa;
            foreach (var cell in _tickingCells)
                cell.SetColor(color);

            yield return null;

            if (alfa < _alfaMinMaxTick.x)
                alfa = _alfaMinMaxTick.y;
            else
                alfa -= _speedTick * Time.deltaTime;
        }
    }
}

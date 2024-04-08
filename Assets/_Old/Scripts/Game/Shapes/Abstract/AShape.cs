using UnityEngine;

[RequireComponent(typeof(ShapeDragging))]
public abstract class AShape : MonoBehaviour
{
    [SerializeField] protected float _timeToStart = 0.3f;

    protected Transform _thisTransform;
    protected ShapeDragging _dragging;
    protected BlocksArea _area;
    protected Vector2Int _oldIndexZeroBlock = Vector2Int.down;
    protected bool _isEnterShape, _controlEnable, _isSpawn;

    public bool ControlEnable { set => _dragging.Enabled = (_controlEnable = value) && IsSpawn; }
    public bool IsSpawn { get => _isSpawn; protected set => _dragging.Enabled = (_isSpawn = value) && _controlEnable; }

    protected virtual void Initialize(BlocksArea area, Bounds boundsGameArea)
    {
        _thisTransform = transform;
        _dragging = GetComponent<ShapeDragging>();

        _dragging.EventStartMove += OnStartMoveShape;
        _dragging.EventMove += OnMoveShape;
        _dragging.EventStopMove += OnStopMoveShape;

        _area = area;
        _dragging.Initialize(boundsGameArea);
    }

    protected abstract void OnStartMoveShape();
    protected abstract void OnMoveShape();
    protected abstract void OnStopMoveShape();
}

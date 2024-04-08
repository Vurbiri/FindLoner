using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ShapeDragging : MonoBehaviour
{
    private Bounds _boundsGameArea;
    private Bounds _boundsMove = new();

    private Camera _mainCamera;
    private Transform _thisTransform;
    private Transform _parent;
    private BoxCollider2D _collider;

    private Vector3 _deltaStart;
    private Vector3 _startLocalPosition;

    private Vector3 _mousePosition;
    private Vector3 _mousePositionOld;

    public bool Enabled { set => _collider.enabled = value; }

    public event Action EventStartMove;
    public event Action EventMove;
    public event Action EventStopMove;

    public void Initialize(Bounds boundsGameArea)
    {
        _collider = GetComponent<BoxCollider2D>();
        _mainCamera = Camera.main;
        _thisTransform = transform;
        _startLocalPosition = transform.localPosition;
        _parent = transform.parent;

        _boundsGameArea = boundsGameArea;
    }

    public void SetSize(Vector3 size)
    {
        _boundsMove = _boundsGameArea;
        _boundsMove.size -= size;
    }

    public void StartWork(Vector3 size)
    {
        SetSize(size);
        Enabled = true;
    }

    public void ReturnToStart()
    {
        transform.parent = _parent;
        _thisTransform.localPosition = _startLocalPosition;
    }

    public IEnumerator ReturnToStartCoroutine(float smoothTime)
    {
        Vector3 velocity = Vector3.zero;

        transform.parent = _parent;
        do
        {
            _thisTransform.localPosition = Vector3.SmoothDamp(_thisTransform.localPosition, _startLocalPosition, ref velocity, smoothTime);
            yield return null;
            smoothTime -= Time.deltaTime;
        }
        while (velocity.sqrMagnitude > 0.01f);

        _thisTransform.localPosition = _startLocalPosition;
    }

    private void OnMouseDown()
    {
        transform.parent = null;
        _mousePositionOld = Input.mousePosition;
        _deltaStart = transform.position - _mainCamera.ScreenToWorldPoint(_mousePositionOld);
        Cursor.visible = false;

        EventStartMove?.Invoke();
    }

    private void OnMouseDrag()
    {
        if((_mousePosition = Input.mousePosition) == _mousePositionOld)
            return;

        _mousePositionOld = _mousePosition;
        _thisTransform.position = _boundsMove.ClosestPoint(_deltaStart +_mainCamera.ScreenToWorldPoint(_mousePosition));
        EventMove?.Invoke();
    }

    private void OnMouseUp()
    {
        Cursor.visible = true;
        EventStopMove?.Invoke();
    }

#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_boundsMove.center, _boundsMove.size);
    }
#endif
}

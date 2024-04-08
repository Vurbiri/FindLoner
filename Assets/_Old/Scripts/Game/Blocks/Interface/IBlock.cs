using UnityEngine;

public interface IBlock
{
    public Vector3 Position { get;}

    public Vector2Int GetIndex();
}

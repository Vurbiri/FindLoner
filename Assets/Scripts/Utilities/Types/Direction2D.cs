using UnityEngine;

public static class Direction2D
{
    public static Vector2Int[] Line { get; } = new Vector2Int[]
    {
        Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left,
    };

    public static Vector2Int[] All { get; } = new Vector2Int[]
    {
        Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left,
        Vector2Int.one, Vector2Int.one * -1, new(1 , -1), new(-1 , 1),
    };
}

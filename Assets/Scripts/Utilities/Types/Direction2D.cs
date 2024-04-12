using UnityEngine;

public static class Direction2D
{
    public static Vector2[] Line { get; } = new Vector2[]
    {
        Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left,
    };

    public static Vector2 Random => Line[UnityEngine.Random.Range(0, Line.Length)];

}

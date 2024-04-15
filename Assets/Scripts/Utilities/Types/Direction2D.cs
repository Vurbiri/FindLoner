using UnityEngine;

public static class Direction2D
{
    public static Vector2 Random => Line[UnityEngine.Random.Range(0, COUNT_DIRECT)];
    public static Vector2Int[] RandomAround => AllArounds[UnityEngine.Random.Range(0, COUNT_AROUND)];

    private static Vector2[] Line { get; } =
    {
        Vector2.up, Vector2.right, Vector2.down, Vector2.left,
    };

    private static Vector2Int[][] AllArounds { get; } =
    {
        new Vector2Int[]{ Vector2Int.up, Vector2Int.right, Vector2Int.down },
        new Vector2Int[]{ Vector2Int.right, Vector2Int.down, Vector2Int.left },
        new Vector2Int[]{ Vector2Int.down, Vector2Int.left, Vector2Int.up },
        new Vector2Int[]{ Vector2Int.left, Vector2Int.up, Vector2Int.right },

        new Vector2Int[]{ Vector2Int.up, Vector2Int.left, Vector2Int.down },
        new Vector2Int[]{ Vector2Int.left, Vector2Int.down, Vector2Int.right },
        new Vector2Int[]{ Vector2Int.down, Vector2Int.right, Vector2Int.up },
        new Vector2Int[]{ Vector2Int.right, Vector2Int.up, Vector2Int.left },
    };

    private const int COUNT_DIRECT = 4;
    private const int COUNT_AROUND = 8;
}

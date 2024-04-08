using UnityEngine;

public static class ExtensionsVector
{
    public static Vector2Int RoundToInt(this Vector3 self) => new(Mathf.RoundToInt(self.x), Mathf.RoundToInt(self.y));
    //public static Vector2Int RoundToInt(this Vector2 self) => new(Mathf.RoundToInt(self.x), Mathf.RoundToInt(self.y));

    public static Vector3 Floor(this Vector3 self) => new(Mathf.Floor(self.x), Mathf.Floor(self.y), Mathf.Floor(self.z));

    //public static Vector2Int FloorToInt(this Vector3 self) => new(Mathf.FloorToInt(self.x), Mathf.FloorToInt(self.y));
    //public static Vector2Int FloorToInt(this Vector2 self) => new(Mathf.FloorToInt(self.x), Mathf.FloorToInt(self.y));

    public static Vector3 ToVector3(this Vector2Int self) => new(self.x, self.y, 0f);
    //public static Vector2 ToVector2(this Vector2Int self) => new(self.x, self.y);

    //public static float RandomRange(this Vector2 self) => Random.Range(self.x, self.y);
}

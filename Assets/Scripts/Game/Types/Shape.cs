using UnityEngine;

public class Shape : AFace<Shape, Sprite[]>
{

    public const int COUNT = 3;

    protected override float Variance => 0.2f;

    public Shape(Sprite[] sprite, Color color) : base(sprite, color) { }
}

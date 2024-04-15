using UnityEngine;

public class Shape : AFace<Shape, Sprite>
{
    protected override float Variance => 0.2f;

    public Shape(Sprite sprite, Color color) : base(sprite, color) { }
}

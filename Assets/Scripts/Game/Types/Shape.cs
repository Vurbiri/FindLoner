using UnityEngine;

public class Shape : AFace<Shape, Sprite>
{
    protected override float Variance => 0.175f;

    public Shape(Sprite sprite, Color color) : base(sprite, color) { }
}

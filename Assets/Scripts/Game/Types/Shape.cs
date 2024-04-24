using UnityEngine;

public class Shape : AFace<Shape, Sprite[]>
{
    public Sprite Main => _value[0];
    public Sprite Center => _value[1];
    public Sprite Top => _value[2];
    public Sprite Left => _value[3];
    public Sprite Bottom => _value[4];
    public Sprite Right => _value[5];

    protected override float Variance => 0.2f;

    public Shape(Sprite[] sprite, Color color) : base(sprite, color) { }
}

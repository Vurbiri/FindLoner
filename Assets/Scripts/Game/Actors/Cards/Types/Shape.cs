using System.Collections.Generic;
using UnityEngine;

public class Shape
{
    public Sprite Sprite { get; private set; }
    public Color Color => _color;
    private Color _color = Color.white;

    private const float VARIANCE = 0.2f;

    public Shape(Sprite sprite, Color color)
    {
        Sprite = sprite;
        _color = color;
    }

    public void SetUniqueColor(List<Shape> otherShapes, float saturationMin, float brightnessMin)
    {
        int count = otherShapes.Count;
        while (count > 0) 
        {
            if(EqualsColor(otherShapes[count - 1].Color))
            {
                _color.Randomize(saturationMin, brightnessMin);
                count = otherShapes.Count;
            }
            else
            {
                count--;
            }
        }

        // Local function
        bool EqualsColor(Color color) => (Mathf.Abs(_color.r - color.r) < VARIANCE && Mathf.Abs(_color.b - color.b) < VARIANCE && Mathf.Abs(_color.g - color.g) < VARIANCE);
    }
}

using System.Collections.Generic;
using UnityEngine;

public abstract class AFace<T, U> where T : AFace<T, U>
{
    public U Value { get; protected set; }
    public Color Color => _color;
    protected Color _color = Color.white;

    protected abstract float Variance {get;}

    public AFace(U value, Color color)
    {
        Value = value;
        _color = color;
    }

    public void SetUniqueColor(IEnumerable<T> otherShapes, float saturationMin, float brightnessMin)
    {
        foreach (T otherShape in otherShapes) 
            while (_color.IsSimilar(otherShape.Color, Variance))
                _color.Randomize(saturationMin, brightnessMin);

    }
}

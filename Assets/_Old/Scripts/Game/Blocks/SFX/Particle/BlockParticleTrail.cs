using UnityEngine;

public class BlockParticleTrail : ABlockParticleSystemController
{
    private Color _color;

    public void Setup(Color color)
    {
        Color = _color = color;
        ClearAndStop();
    }

    public void SetColorAlfa(float alfa)
    {
        _color.a = alfa;
        Color = _color;
    }
}

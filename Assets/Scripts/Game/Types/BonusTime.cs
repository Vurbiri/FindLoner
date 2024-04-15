using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BonusTime : AFace<BonusTime, int>
{
    protected override float Variance => 0.15f;

    public BonusTime(int time, Color color) : base(time, color) { }
}

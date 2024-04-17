using UnityEngine;

public class BonusTime : AFace<BonusTime, int>
{
    protected override float Variance => 0.14f;

    public BonusTime(int time, Color color) : base(time, color) { }
}

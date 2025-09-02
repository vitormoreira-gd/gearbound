using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EquipStats : Stats
{
    public List<ScalingAttribute> scalings = new();

    public EquipStats(StatsType type, float value) : base(type, value)
    {
    }
}

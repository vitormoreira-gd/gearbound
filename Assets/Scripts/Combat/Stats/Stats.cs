using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Stats
{
    public StatsType type;
    public float Value => baseValue + modifiers.Sum();

    [SerializeField] private float baseValue;
    [SerializeField] private List<float> modifiers;

    public Stats(
        StatsType type,
        float value
        )
    {
        this.type = type;
        baseValue = value;
    }

    public void AddModifier(float value)
    {
        modifiers.Add(value);
    }

    public void RemoveModifier(float value)
    {
        modifiers.Remove(value);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Stats
{
    public StatsType type;
    public float Value
    {
        get
        {
            if(modifiers != null && modifiers.Count > 0)
            {
                return baseValue + modifiers.Sum();
            }

            modifiers = new();
            return baseValue;
        }
    }

    [SerializeField] private float baseValue;
    private List<float> modifiers = new();

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
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
                return _baseValue + modifiers.Sum();
            }

            modifiers = new();
            return _baseValue;
        }
    }
    public float BaseValue => _baseValue;

    [SerializeField] private float _baseValue;
    private List<float> modifiers = new();

    public Stats(
        StatsType type,
        float value
        )
    {
        this.type = type;
        _baseValue = value;
    }

    public void SetBaseValue(float value) => _baseValue = value;

    public void AddModifier(float value)
    {
        modifiers.Add(value);
    }

    public void RemoveModifier(float value)
    {
        modifiers.Remove(value);
    }
}
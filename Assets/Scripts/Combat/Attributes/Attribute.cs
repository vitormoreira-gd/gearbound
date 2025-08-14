using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Attribute
{
    public AttributeType type;
    public float Value => baseValue + modfiers.Sum();
        
    [SerializeField] private float baseValue;
    [SerializeField] private List<float> modfiers;

    public Attribute(
        AttributeType type,
        float value)
    {
        this.type = type;
        this.baseValue = value;
    }
        
    public void AddModifier(float value)
    {
        modfiers.Add(value);
    }

    public void RemoveModifier(float value)
    {
        modfiers.Remove(value);
    }
}
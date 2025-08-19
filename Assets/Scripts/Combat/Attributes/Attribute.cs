using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Attribute
{
    public AttributeType type;
    public int Level => baseLevel + modfiers.Sum();
        
    [SerializeField] private int baseLevel;
    private List<int> modfiers = new();

    public Attribute(
        AttributeType type,
        int value)
    {
        this.type = type;
        this.baseLevel = value;
    }
        
    public void AddModifier(int value)
    {
        modfiers.Add(value);
    }

    public void RemoveModifier(int value)
    {
        modfiers.Remove(value);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Attribute
{
    public AttributeType type;
    public int Level => _baseLevel + (modfiers?.Sum() ?? 0);
    public int BaseLevel => _baseLevel;
        
    [SerializeField] private int _baseLevel;
    private List<int> modfiers = new();

    public Attribute(
        AttributeType type,
        int value)
    {
        this.type = type;
        this._baseLevel = value;
    }

    public void SetBaseLevel(int value) => _baseLevel = value;
        
    public void AddModifier(int value)
    {
        modfiers.Add(value);
    }

    public void RemoveModifier(int value)
    {
        modfiers.Remove(value);
    }
}
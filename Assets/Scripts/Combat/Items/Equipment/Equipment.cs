using System;
using System.Collections.Generic;

[Serializable]
public class Equipment : Item
{
    public EquipType slot;
    public List<Stats> stats = new();
    public List<ScalingAttribute> scalings = new();

    public Equipment(
        string name, 
        string description,
        ItemType type,
        EquipType slot,
        List<Stats> stats,
        List<ScalingAttribute> scalings) : base (name, description, type)
    {
        this.type = ItemType.Equipment;
        this.slot = slot;
        this.stats = new(stats);
        this.scalings = scalings;
    }
}
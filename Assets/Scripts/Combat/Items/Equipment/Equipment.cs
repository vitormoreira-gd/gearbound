using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Equipment : Item
{
    public EquipType slot;
    public List<EquipStats> stats = new();
    //public List<ScalingAttribute> scalings = new();

    public Equipment(
        string name, 
        string description,
        ItemType type,
        EquipType slot,
        List<EquipStats> stats) : base (name, description, type)
        //List<ScalingAttribute> scalings) 
    {
        this.type = ItemType.Equipment;
        this.slot = slot;
        this.stats = new(stats);
        //this.scalings = scalings;
    }
}
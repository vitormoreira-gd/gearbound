using System;
using System.Collections.Generic;

[Serializable]
public class Creature
{
    public string name;
    public CreatureType Type;
    public List<Attribute> attributes;
    public List<Stats> stats;

    public Creature(
        string name,
        List<Attribute> attributes,
        List<Stats> stats)
    {
        this.attributes = attributes;
        this.stats = stats;
    }
}

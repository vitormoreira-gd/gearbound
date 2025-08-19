using System;
using System.Collections.Generic;
using System.Linq;


[Serializable]
public class Creature
{
    public string name;
    public CreatureType type;
    public List<Attribute> attributes = new();
    public Stats[] stats = new Stats[]
    {
        new(type: StatsType.Health,         value: 100f),
        new(type: StatsType.Regen,          value: 10f),
        new(type: StatsType.Attack,         value: 10f),
        new(type: StatsType.Speed,          value: 10f),
        new(type: StatsType.AttackSpeed,    value: 0.5f)
    };
    public List<Equipment> equipment = new();

    public Creature(
        string name,
        CreatureType type,
        List<Attribute> attributes)
    {
        this.name = name;
        this.type = type;
        this.attributes = attributes;

        foreach (StatsType stats in Enum.GetValues(typeof(StatsType))) 
        {
            AttributeType attributeType = stats switch
            {
                StatsType.Health        => AttributeType.Vigor,
                StatsType.Regen         => AttributeType.Vigor,
                StatsType.Attack        => AttributeType.Power,
                StatsType.Speed         => AttributeType.Agility,
                StatsType.AttackSpeed   => AttributeType.Agility,
                _ => AttributeType.Vigor
            };

            Attribute attribute = attributes.FirstOrDefault(a => a.type == attributeType);
            float statsValue = StatsData.GettStatsByLevel(stats, attribute.Level);
            int statsIndex = this.stats.ToList().FindIndex(s => s.type == stats);

            this.stats[statsIndex] = new Stats(stats, statsValue);
        }
    }

    public void AddEquipment(Equipment equip)
    {
        foreach (Stats stats in equip.stats)
        {
            int index = this.stats.ToList().FindIndex(s => s.type == stats.type);

            this.stats[index].AddModifier(stats.Value);
            UnityEngine.Debug.Log(stats.Value);
        }

        foreach (ScalingAttribute scaling in equip.scalings)
        {
            float scalingFactor = scaling.type switch
            {
                ScalingType.S => 2f,
                ScalingType.A => 1.75f,
                ScalingType.B => 1.5f,
                ScalingType.C => 1.2f,
                _ => 1,
            };
            float attribute = this.attributes.Find(a => a.type == scaling.attribute).Level;
            float modifierValue = MathF.Pow(attribute, scalingFactor);

            int index = this.stats.ToList().FindIndex(s => s.type == scaling.stats);

            Stats creatureStats = this.stats[index];

            UnityEngine.Debug.Log(modifierValue);
            creatureStats.AddModifier(modifierValue);
        }

        equipment.Add(equip);

        UnityEngine.Debug.Log(StatsDebug());
    }

    public string StatsDebug()
    {
        string d = "";

        foreach (Stats s in stats)
        {
            d += $"{s.type} :: {s.Value}\n";
        }

        return d;
    }
}
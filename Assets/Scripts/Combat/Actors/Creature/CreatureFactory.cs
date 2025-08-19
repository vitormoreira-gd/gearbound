using System.Linq;

public static class CreatureFactory
{
    public static Creature CreateCreature(
        string name,
        CreatureType creatureType,
        int vigorLevel,
        int powerLevel,
        int agilityLevel)
    {
        Attribute[] creatureAttributes = new Attribute[]
        {
            new(type: AttributeType.Vigor,      value: vigorLevel),
            new(type: AttributeType.Power,      value: powerLevel),
            new(type: AttributeType.Agility,    value: agilityLevel)
        };

        Creature creature = new Creature(
            name: name,
            type: CreatureType.Player,
            attributes: creatureAttributes.ToList()
            );

        return creature;
    }
}

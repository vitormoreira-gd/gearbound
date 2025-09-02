using UnityEngine;

public class TEST_CreatureRuntime : MonoBehaviour
{
    public int vigorLevel = 10;
    public int powerLevel = 10;
    public int agilityLevel = 10;
    public Creature Player;

    public Equipment equip;

    private void Awake()
    {
        StatsData.LoadStats();
    }

    private void Start()
    {
        Player = CreatureFactory.CreateCreature(
            name: "Player",
            creatureType: CreatureType.Player,
            vigorLevel: this.vigorLevel,
            powerLevel: this.powerLevel,
            agilityLevel: this.agilityLevel);
    }

    [ContextMenu("Add Equip")]
    private void AddEquip()
    {
        Equipment newEquip = new Equipment(equip.name, equip.description, equip.type, equip.slot, equip.stats/*, equip.scalings*/);

        Player.AddEquipment(newEquip);
    }
}

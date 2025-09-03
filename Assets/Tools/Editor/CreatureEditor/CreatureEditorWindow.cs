using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class CreatureEditorWindow : EditorWindow
{
    private const string JsonPath = "Assets/Resources/Creatures/creatures.json";

    [System.Serializable]
    private class CreatureListWrapper
    {
        public List<Creature> creatures = new();
    }

    private CreatureListWrapper creatureData;
    private Vector2 scroll;
    private int selectedIndex = -1;

    [MenuItem("Game Tools/Creature Editor")]
    public static void Open()
    {
        GetWindow<CreatureEditorWindow>("Creature Editor");
    }

    private void OnEnable()
    {
        LoadJson();
    }

    private void LoadJson()
    {
        if (!File.Exists(JsonPath))
        {
            creatureData = new CreatureListWrapper();
            return;
        }

        string json = File.ReadAllText(JsonPath);
        creatureData = JsonUtility.FromJson<CreatureListWrapper>(json);
        creatureData ??= new CreatureListWrapper();
    }

    private void SaveJson()
    {
        string json = JsonUtility.ToJson(creatureData, true);
        File.WriteAllText(JsonPath, json);
        AssetDatabase.Refresh();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        scroll = EditorGUILayout.BeginScrollView(scroll);

        if (selectedIndex == -1)
        {
            DrawCreatureList();
        }
        else
        {
            DrawCreatureEditor(creatureData.creatures[selectedIndex]);
        }

        EditorGUILayout.EndScrollView();

        GUILayout.FlexibleSpace();

        if(GUILayout.Button("Salvar JSON"))
        {
            SaveJson();
        }
    }

    private void DrawCreatureList()
    {
        EditorGUILayout.LabelField("Creatures", EditorStyles.boldLabel);

        for (int i = 0; i < creatureData.creatures.Count; i++)
        {
            var creature = creatureData.creatures[i];

            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.LabelField($"{creature.name} ({creature.type})");

            if (GUILayout.Button("Editar", GUILayout.Width(60)))
            {
                selectedIndex = i;
            }

            if (GUILayout.Button("Remover", GUILayout.Width(70)))
            {
                creatureData.creatures.RemoveAt(i);
                SaveJson();
                break;
            }

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Adicionar Nova Criatura"))
        {
            var newCreature = new Creature("New Creature", CreatureType.Humanoid, new List<Attribute>());
            newCreature.aiConfig = new CreatureAIConfig();
            creatureData.creatures.Add(newCreature);
            selectedIndex = creatureData.creatures.Count - 1;
        }
    }

    private void DrawCreatureEditor(Creature creature)
    {
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.LabelField("Editar Criatura", EditorStyles.boldLabel);

        creature.name = EditorGUILayout.TextField("Nome", creature.name);
        creature.type = (CreatureType)EditorGUILayout.EnumPopup("Tipo", creature.type);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Attributes", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();
        for (int i = 0; i < creature.attributes.Count; i++)
        {
            var attr = creature.attributes[i];
            EditorGUILayout.BeginHorizontal();
            attr.type = (AttributeType)EditorGUILayout.EnumPopup(attr.type, GUILayout.Width(120));
            attr.SetBaseLevel(EditorGUILayout.IntField(attr.BaseLevel, GUILayout.Width(50)));
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                creature.attributes.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Adicionar Attribute"))
        {
            creature.attributes.Add(new Attribute(AttributeType.Vigor, 1));
        }

        // Se houve mudança nos atributos - recalcula os stats
        if (EditorGUI.EndChangeCheck())
        {
            UpdateStatsFromAttributes(creature);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Stats", EditorStyles.boldLabel);

        for (int i = 0; i < creature.stats.Length; i++)
        {
            var stat = creature.stats[i];
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(stat.type.ToString(), GUILayout.Width(120));
            stat.SetBaseValue(EditorGUILayout.FloatField(stat.BaseValue, GUILayout.Width(60)));
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("AI Config", EditorStyles.boldLabel);
        creature.aiConfig.perceptionType = EditorGUILayout.TextField("Perception", creature.aiConfig.perceptionType);
        creature.aiConfig.perceptionRange = EditorGUILayout.FloatField("Range", creature.aiConfig.perceptionRange);
        creature.aiConfig.targetingRule = EditorGUILayout.TextField("Targeting", creature.aiConfig.targetingRule);
        creature.aiConfig.pathProvider = EditorGUILayout.TextField("Path", creature.aiConfig.pathProvider);
        creature.aiConfig.avoidance = EditorGUILayout.TextField("Avoidance", creature.aiConfig.avoidance);

        EditorGUILayout.Space();

        if (GUILayout.Button("Voltar"))
        {
            selectedIndex = -1;
        }

        EditorGUILayout.EndVertical();
    }


    private void UpdateStatsFromAttributes(Creature creature)
    {
        foreach (StatsType stats in System.Enum.GetValues(typeof(StatsType)))
        {
            AttributeType attributeType = stats switch
            {
                StatsType.Health => AttributeType.Vigor,
                StatsType.Regen => AttributeType.Vigor,
                StatsType.Attack => AttributeType.Power,
                StatsType.Speed => AttributeType.Agility,
                StatsType.AttackSpeed => AttributeType.Agility,
                _ => AttributeType.Vigor
            };

            Attribute attribute = creature.attributes.FirstOrDefault(a => a.type == attributeType);
            if (attribute != null)
            {
                float statsValue = StatsData.GetStatsByLevel(stats, attribute.Level);
                int statsIndex = creature.stats.ToList().FindIndex(s => s.type == stats);
                if (statsIndex >= 0)
                {
                    creature.stats[statsIndex].SetBaseValue(statsValue);
                }
            }
        }
    }

}

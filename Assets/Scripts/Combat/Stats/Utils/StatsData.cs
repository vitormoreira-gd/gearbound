using System.Collections.Generic;
using UnityEngine;

public static class StatsData
{
    public static IReadOnlyList<float> Health => _statsTable.health.AsReadOnly();
    public static IReadOnlyList<float> Regen => _statsTable.regen.AsReadOnly();
    public static IReadOnlyList<float> Attack => _statsTable.attack.AsReadOnly();
    public static IReadOnlyList<float> Speed => _statsTable.speed.AsReadOnly();
    public static IReadOnlyList<float> AttackSpeed => _statsTable.attackspeed.AsReadOnly();

    private static StatsTable _statsTable;

    public static void LoadStats()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Stats/stats_table");

        if(jsonFile == null)
        {
            Debug.LogError("Erro: Arquivo 'Stats/stats_table.json' não encontrado na pasta Resources.");
        }

        _statsTable = JsonUtility.FromJson<StatsTable>(jsonFile.text);
        Debug.Log("Tabela de Stats carregadas com sucesso.");
    }

    public static float GettStatsByLevel(StatsType type, int level)
    {
        float result = type switch
        {
            StatsType.Health        => Health[level],
            StatsType.Regen         => Regen[level],
            StatsType.Attack        => Attack[level],
            StatsType.Speed         => Speed[level],
            StatsType.AttackSpeed   => AttackSpeed[level],
            _ => Health[0]
        };

        return result;
    }
}

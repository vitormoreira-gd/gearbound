using System;
using UnityEngine;

[Serializable]
public class CreatureAIConfig
{
    public string perceptionType;
    public float perceptionRange;

    public string targetingRule;
    public string pathProvider;
    public string avoidance;
}

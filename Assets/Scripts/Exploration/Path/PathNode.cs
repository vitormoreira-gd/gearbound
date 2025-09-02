using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PathNode
{
    public string id;
    public Vector3 position;

    public List<string> nextPossibleNodes = new();

    public PathNode(string name, Vector3 position)
    {
        this.id = name;
        this.position = position;
    }

    public void SetNextPossibleNodes(List<string> nodesId)
    {
        nextPossibleNodes.Clear();
        nextPossibleNodes = new(nodesId);
    }

    public void UpdateNodePosition(Vector3 position)
    {
        this.position = position;
    }
}
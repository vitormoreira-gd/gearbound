using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class PathCreator : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private bool drawGizmos = true;
    [SerializeField] private Color nodeColor = Color.yellow;
    [SerializeField] private Color connectionColor = Color.red;
    [SerializeField] private float nodeSize = 0.2f;

    private PathDefinition pathDef;
    private List<Transform> children = new(); // if -> transform
    private Dictionary<string, Transform> nodeMap = new();
    private Vector3 offset = new Vector3(0.5f, 0f, 0.5f);

    public void Initialize(PathDefinition path)
    {
        ClearChildren();
        pathDef = path;

        foreach (PathNode node in path.nodes)
        {
            CreateNodeGameObject(node);
        }

        UpdateNodeMap();
    }

    public void SelectNode(string id)
    {
        Selection.activeGameObject = nodeMap[id].gameObject;
    }

    public void AddNode()
    {
        if(pathDef == null)
        {
            return;
        }

        string newId = $"{pathDef.nodes.Count + 1}";
        Vector3 newPos = children.Count > 0 ? children[^1].position + offset : transform.position;

        PathNode newNode = new PathNode(newId, newPos);
        pathDef.nodes.Add(newNode);

        CreateNodeGameObject(newNode);
        UpdateNodeMap();

        Selection.activeGameObject = nodeMap[newId].gameObject;
    }

    public void SavePath()
    {
        if (pathDef == null)
        {
            return;
        }

        foreach (PathNode node in pathDef.nodes)
        {
            if(nodeMap.TryGetValue(node.id, out Transform t))
            {
                node.UpdateNodePosition(t.position);
            }

            node.nextPossibleNodes.RemoveAll(string.IsNullOrEmpty);
            node.nextPossibleNodes = new List<string>(new HashSet<string>(node.nextPossibleNodes));
        }
    }

    private void CreateNodeGameObject(PathNode node)
    {
        GameObject go = new GameObject(node.id);
        go.transform.parent = transform;
        go.transform.position = node.position;
        children.Add(go.transform);
    }

    private void ClearChildren()
    {
        children.Clear();

        foreach (Transform child in children)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    private void UpdateNodeMap()
    {
        nodeMap.Clear();

        foreach(Transform child in children)
        {
            nodeMap[child.name] = child;
        }
    }

    private void OnDrawGizmos()
    {
        if(!drawGizmos || pathDef == null)
        {
            return;
        }

        Gizmos.color = nodeColor;

        foreach (Transform child in children)
        {
            Gizmos.DrawSphere(child.position, nodeSize);
        }

        Gizmos.color = connectionColor;

        foreach (PathNode node in pathDef.nodes)
        {
            if(!nodeMap.TryGetValue(node.id, out Transform fromTransform))
            {
                continue;
            }

            foreach (string nextId in node.nextPossibleNodes)
            {
                if(nodeMap.TryGetValue(nextId, out Transform toTransform))
                {
                    Gizmos.DrawLine(fromTransform.position, toTransform.position);
                }
            }
        }
    }

    private void OnValidate()
    {
        UpdateNodeMap();
    }
}

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using PlasticPipe.PlasticProtocol.Messages;

public class PathCreatorWindow : EditorWindow
{
    private string JsonPath = "Assets/Resources/Exploration/paths.json";

    [System.Serializable]
    public class PathDefinitionWrapper
    {
        public List<PathDefinition> _paths = new();
    }

    public PathDefinitionWrapper pathData;
    private PathCreator pathCreator;
    private Vector2 generalScroll;
    private Vector2 nodesScroll;
    private Vector2 conectionsScroll;
    private int selectedIndex = -1;
    private bool isEditing;
    private int selectedNodeIndex = -1;

    [MenuItem("Game Tools/Path Creator")]
    public static void Open()
    {
        GetWindow<PathCreatorWindow>("Path Creation Tool");
    }

    private void OnEnable()
    {
        LoadJson();
    }

    private void OnDisable()
    {
        DeletePathCreator();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();

        if (isEditing)
        {
            DrawEditingMode();
            return;
        }

        DrawPaths();
        GUILayout.FlexibleSpace();
    }

    private void LoadJson()
    {
        if (!File.Exists(JsonPath))
        {
            pathData = new PathDefinitionWrapper();
            return;
        }

        string json = File.ReadAllText(JsonPath);
        pathData = JsonUtility.FromJson<PathDefinitionWrapper>(json);
        pathData ??= new PathDefinitionWrapper();
    }

    private void SaveJson()
    {
        string json = JsonUtility.ToJson(pathData, true);
        File.WriteAllText(JsonPath, json);
        AssetDatabase.Refresh();
        GUI.FocusControl(null);
    }

    private void FindOrCreatePathCreator()
    {
        pathCreator = Object.FindAnyObjectByType<PathCreator>();

        if (pathCreator == null)
        {
            GameObject go = new GameObject("PathCreator");
            pathCreator = go.AddComponent<PathCreator>();
        }
    }

    private void DeletePathCreator()
    {
        if (pathCreator == null)
        {
            return;
        }

        GameObject pathCreatorGo = Object.FindAnyObjectByType<PathCreator>().gameObject;
        DestroyImmediate(pathCreatorGo);
        pathCreator = null;
    }

    private void DrawPaths()
    {
        EditorGUILayout.LabelField("Paths: ", EditorStyles.boldLabel);

        generalScroll = EditorGUILayout.BeginScrollView(generalScroll);
        EditorGUILayout.BeginVertical();

        for (int i = 0; i < pathData._paths.Count; i++)
        {
            if (GUILayout.Button($"{pathData._paths[i].name}"))
            {
                selectedIndex = i;
                FindOrCreatePathCreator();
                InitializePathCreator(pathData._paths[i]);
                isEditing = true;

                selectedNodeIndex = 0;
                GUI.FocusControl(null);
            }
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }

    private void InitializePathCreator(PathDefinition path)
    {
        pathCreator.Initialize(path);
    }

    private void DrawEditingMode()
    {
        if(selectedIndex < 0 || selectedIndex >= pathData._paths.Count)
        {
            isEditing = false;
            return;
        }

        PathDefinition currentPath = pathData._paths[selectedIndex];

        EditorGUILayout.LabelField($"Editando :: {currentPath.name} ::", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        currentPath.name = EditorGUILayout.TextField("Name: ", currentPath.name);
        EditorGUILayout.Space();

        generalScroll = EditorGUILayout.BeginScrollView(generalScroll);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("<-"))
        {
            isEditing = false;
            selectedNodeIndex = -1;
            DeletePathCreator();
        }
        if (GUILayout.Button("Add Node"))
        {
            pathCreator.AddNode();
            selectedNodeIndex = currentPath.nodes.Count - 1;
            GUI.FocusControl(null);
        }
        if (GUILayout.Button("Save Path"))
        {
            pathCreator.SavePath();
            SaveJson();
            Debug.Log($"Path: {currentPath.name} salvo no JSON.");
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();
        EditorGUILayout.Space();

        if (selectedNodeIndex >= 0 && selectedNodeIndex < currentPath.nodes.Count)
        {
            DrawNodeEditor(currentPath.nodes[selectedNodeIndex], currentPath.nodes);
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Nodes", EditorStyles.boldLabel);
        nodesScroll = EditorGUILayout.BeginScrollView(nodesScroll);
        for (int i = 0; i < currentPath.nodes.Count; i++)
        {
            if (GUILayout.Button($"Node: {currentPath.nodes[i].id}"))
            {
                selectedNodeIndex = i;
                pathCreator.SelectNode(currentPath.nodes[i].id);
                GUI.FocusControl(null);
            }
        }
        EditorGUILayout.EndScrollView();
    }

    private void DrawNodeEditor(PathNode node, List<PathNode> allNodes)
    {
        EditorGUILayout.LabelField("Edit Node: ", EditorStyles.boldLabel);

        string oldId = node.id;
        string newId = EditorGUILayout.TextField("Name: ", node.id);

        if (newId != oldId)
        {
            node.id = newId;

            foreach (PathNode other in allNodes)
            {
                for (int i = 0; i < other.nextPossibleNodes.Count; i++)
                {
                    if (other.nextPossibleNodes[i] == oldId)
                    {
                        other.nextPossibleNodes[i] = newId;
                    }
                }
            }

            if(pathCreator != null)
            {
                Transform t = pathCreator.transform.Find(oldId);
                if(t != null)
                {
                    t.name = newId;
                }
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Next Possible Nodes: ", EditorStyles.boldLabel);

        conectionsScroll = EditorGUILayout.BeginScrollView(conectionsScroll, GUILayout.Height(200));
        for (int i = 0; i < allNodes.Count; i++)
        {
            if (allNodes[i] == node)
            {
                continue;
            }

            bool isConnected = node.nextPossibleNodes.Contains(allNodes[i].id);
            bool newState = EditorGUILayout.Toggle(allNodes[i].id, isConnected);

            if(newState && !isConnected)
            {
                node.nextPossibleNodes.Add(allNodes[i].id);
            }
            else if (!newState && isConnected)
            {
                node.nextPossibleNodes.Remove(allNodes[i].id);
            }
        }
        EditorGUILayout.EndScrollView();
    }
}

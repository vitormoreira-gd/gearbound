using System.Collections.Generic;
using UnityEngine;

public static class PathLoader
{
    private static List<PathDefinition> cachedPaths = new();

    public static PathDefinition GetPathByName(string name)
    {
        //if (cachedPaths == null)
        //{
            TextAsset json = Resources.Load<TextAsset>("Exploration/paths");

            if (json == null)
            {
                Debug.LogError("Paths JSON not found in Resources/Exploration/paths.json");
                return null;
            }

            cachedPaths = JsonUtility.FromJson<PathList>(json.text).paths;
        //}

        return cachedPaths.Find(p => p.name == name);
    }

    [System.Serializable]
    private class PathList
    {
        public List<PathDefinition> paths;
    }
}

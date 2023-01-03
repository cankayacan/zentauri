using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelRoot
{
    public LevelGameObject[] gameObjects;
}

[System.Serializable]
public class LevelGameObject
{
    public string name;
    public Vector3 position;
    public Vector3 rotation;
}

public class LevelManager: MonoBehaviour
{
    public PlayerManager playerManager;
    
    public GameObject ball;

    public GameObject target;

    private Dictionary<string, GameObject> gameObjectNameMap;

    public void Awake()
    {
        gameObjectNameMap = new Dictionary<string, GameObject>()
        {
            { "ball", ball },
            { "spawnPoint", new GameObject() },
            { "target", target },
        };
    }
    
    private void Start()
    {
        var jsonFile = (TextAsset)Resources.Load("level-1", typeof(TextAsset));
        var levelRoot = JsonUtility.FromJson<LevelRoot>(jsonFile.text);

        foreach (var gameObjectJson in levelRoot.gameObjects)
        {
            var levelGameObject = gameObjectNameMap[gameObjectJson.name];
            var (x, y, z) = gameObjectJson.rotation;
            var position = gameObjectJson.position;
            var quaternion = Quaternion.Euler(x, y, z);
            Instantiate(levelGameObject, position, quaternion);

            if (gameObjectJson.name == "spawnPoint")
            {
                playerManager.StartPlayer(position, quaternion);
            }
        }
    }
}

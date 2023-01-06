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
    private int currentLevel;

    public PlayerManager playerManager;
    
    public GameObject ball;

    public GameObject target;

    private Dictionary<string, GameObject> gameObjectNameMap;

    public void Awake()
    {
        gameObjectNameMap = new Dictionary<string, GameObject>
        {
            { "ball", ball },
            { "spawnPoint", new GameObject() },
            { "target", target },
        };
    }
    
    private void Start()
    {
        var jsonFile = (TextAsset)Resources.Load($"levels/level-{LevelUtils.CurrentLevel}", typeof(TextAsset));

        if (jsonFile == null)
        {
            GameController.Default.PublishLevelError("Error in loading the next level, you might have finished all levels!");
            return;
        }

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

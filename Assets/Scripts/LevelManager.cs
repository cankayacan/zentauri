using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class LevelRoot
{
    public LevelGameObject[] gameObjects;
}

public class LevelComponent
{
    public string name;
    public dynamic props;
}

[System.Serializable]
public class LevelGameObject
{
    public string name;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    public List<LevelComponent> components;
}

public class LevelManager : MonoBehaviour
{
    private int currentLevel;

    public PlayerManager playerManager;

    public GameObject ball;

    public GameObject target;

    public GameObject key;

    public GameObject star;

    public GameObject coin;

    public GameObject wall;

    private Dictionary<string, GameObject> gameObjectNameMap;

    public void Awake()
    {
        gameObjectNameMap = new Dictionary<string, GameObject>
        {
            { "ball", ball },
            { "spawnPoint", new GameObject() },
            { "target", target },
            { "key", key },
            { "star", star },
            { "coin", coin },
            { "wall", wall }
        };
    }

    private void Start()
    {
        var jsonFile = (TextAsset)Resources.Load($"levels/level-{LevelUtils.CurrentLevel}", typeof(TextAsset));

        if (jsonFile == null)
        {
            GameController.Default.PublishLevelError(
                "Error in loading the next level, you might have finished all levels!");
            return;
        }

        var levelRoot = JsonConvert.DeserializeObject<LevelRoot>(jsonFile.text);

        foreach (var gameObjectJson in levelRoot.gameObjects)
        {
            var instantiatedGameObject = InstantiateGameObject(gameObjectJson);

            foreach (var componentJson in gameObjectJson.components)
            {
                InstantiateComponent(instantiatedGameObject, componentJson);
            }
        }
    }

    private GameObject InstantiateGameObject(LevelGameObject gameObjectJson)
    {
        var gameObjectPrefab = gameObjectNameMap[gameObjectJson.name];

        var position = gameObjectJson.position;

        var (x, y, z) = gameObjectJson.rotation;
        var quaternion = Quaternion.Euler(x, y, z);
        
        var instantiatedGameObject = Instantiate(gameObjectPrefab, position, quaternion);
        instantiatedGameObject.transform.localScale = gameObjectJson.scale;
        
        if (gameObjectJson.name == "spawnPoint")
        {
            playerManager.StartPlayer(position, quaternion);
        }

        return instantiatedGameObject;
    }

    private static void InstantiateComponent(GameObject instantiatedGameObject, LevelComponent component)
    {
        if (component.name == "MoveBackAndForth")
        {
            InstantiateMoveBackAndForth(instantiatedGameObject, component);
        }
    }

    private static void InstantiateMoveBackAndForth(GameObject instantiatedGameObject, LevelComponent component)
    {
        var moveBackAndForthComponent = instantiatedGameObject.AddComponent<MoveBackAndForth>();
        var start = component.props.start;
        moveBackAndForthComponent.start = new Vector3((float)start.x, (float)start.y, (float)start.z);

        var end = component.props.end;
        moveBackAndForthComponent.end = new Vector3((float)end.x, (float)end.y, (float)end.z);

        moveBackAndForthComponent.speed = component.props.speed;
    }
}
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class LevelRoot
{
    public LevelGameObject[] gameObjects;
}

public abstract class LevelComponent
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
    private const string SpawnPointGameObjectName = "SpawnPoint";

    private int currentLevel;

    public PlayerManager playerManager;

    public GameObject ball;

    public GameObject key;

    public GameObject star;

    public GameObject coin;

    public GameObject wall2X301;

    public GameObject wallChamfered5X501;

    private Dictionary<string, GameObject> gameObjectNameMap;

    public void Awake()
    {
        gameObjectNameMap = new Dictionary<string, GameObject>
        {
            { "Ball", ball },
            { SpawnPointGameObjectName, new GameObject() },
            { "SM_Icon_Key_01", key },
            { "SM_Icon_Star_02", star },
            { "SM_Icon_Coin_02", coin },
            { "SM_Buildings_Wall_2x3_01P", wall2X301 },
            { "SM_Buildings_WallChamfered_5x5_01", wallChamfered5X501 }
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
        
        if (gameObjectJson.name == SpawnPointGameObjectName)
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
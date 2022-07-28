using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Default
    {
        get
        {
            if (instance != null) return instance;

            instance = FindObjectOfType<T> ();
            if (instance != null) return instance;

            var obj = new GameObject ();
            obj.name = typeof(T).Name;
            instance = obj.AddComponent<T>();
            return instance;
        }
    }

    public virtual void Awake ()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy (gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        Destroy (gameObject);
    }
}

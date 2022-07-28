using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static bool isAppQuiting;
    private static T instance;

    public static T Default
    {
        get
        {
            if (instance != null)
            {
                Debug.Log("Returning existing instance");
                return instance;
            }

            instance = FindObjectOfType<T> ();
            if (instance != null)
            {
                Debug.Log("Found object");
                return instance;
            }

            Debug.Log("Creating new game object");
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
            Debug.Log("Awake setting instance");
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Debug.Log("Awake destroying");
            Destroy (gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        isAppQuiting = true;
    }
}

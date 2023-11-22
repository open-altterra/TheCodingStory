using System;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }
    public virtual void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning($"Only one Singleton can exist in a scene. The component '{this.GetType().Name}' has been removed from '{this.name}' game object.", this.gameObject);
            Destroy(this);
            return;
        }

        Instance = this as T;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager<T> : MonoBehaviour where T : Manager<T>
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError(typeof(T).ToString() + " does not exist");
            }
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this as T;
        Init();

    }

    public virtual void Init()
    {
        //optional
    }
}

using System.Collections.Generic;
using UnityEngine;

public class BaseManager<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T I
    {
        get
        {
            if(instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
            }
            return instance;
        }
        protected set
        {
            instance = value;
        }
    }
    [SerializeField]
    bool dontDestroyOnLoad = false;

    protected virtual void Awake()
    {
        Inisialize();
    }

    protected virtual void OnLevelWasLoaded(int level)
    {
        Inisialize();
    }

    protected void Inisialize()
    {
        List<T> instances = new List<T>();
        instances.AddRange((T[])FindObjectsOfType(typeof(T)));

        if (I == null) I = instances[0];
        instances.Remove(I);

        if (instances.Count == 0) return;
        //あぶれ者のinstanceはデストロイ 
        foreach (T t in instances) Destroy(t.gameObject);
    }

    protected virtual void Start()
    {
        if(dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        I = null;
    }
}

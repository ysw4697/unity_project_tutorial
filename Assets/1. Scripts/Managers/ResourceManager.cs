using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject Prefab = this.Load<GameObject>($"Prefabs/{path}");

        if (parent == null)
        {
            Debug.LogError($"Can't find parent of {path}");
            return null;
        }
        
        return Object.Instantiate(Prefab, parent);
    }

    public void Destroy(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }
        Object.Destroy(obj);
    }
}

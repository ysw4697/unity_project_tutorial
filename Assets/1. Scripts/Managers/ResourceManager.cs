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
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            Debug.LogError($"Failed to load prefab {path}");
            return null;
        }
        
        // ResourceManager의 Instantiate가 아닌 유니티에서 기본 제공하는 Instantiate를 사용하기 위해 앞에 Object.를 붙임
        return Object.Instantiate(prefab, parent);
    }

    public void Destroy(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }
        
        Object.Destroy(obj);
    }

    // 오버로딩
    public void Destroy(GameObject obj, float time)
    {
        if (obj == null)
        {
            return;
        }
        
        Object.Destroy(obj, time);
    }
}

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
        // 1. original을 이미 들고 있다면 바로 사용
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");

        if (prefab == null)
        {
            Debug.LogError($"Can't find parent of {path}");
            return null;
        }
        
        // 2. 이미 풀링된 객체가 있는지 확인 후 생성
        GameObject go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;
        
        return go;
    }

    public void Destroy(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }
        
        // 만약에 풀링이 필요한 객체라면 풀링 매니저에 위탁
        Object.Destroy(obj);
    }
}

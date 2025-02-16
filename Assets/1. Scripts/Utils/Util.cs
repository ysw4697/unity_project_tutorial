using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
        {
            component = go.AddComponent<T>();
        }
        
        return component;
    }
    
    public static GameObject FindChild(GameObject obj, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(obj, name, recursive);
        if (transform == null)
        {
            return null;
        }
        
        return transform.gameObject;
    }
    
    //            탐색을 시작할 최상위 부모의 이름, 찾으려는 자식 개체의 이름, 자식 개체의 모든 하위 개체 탐색 여부
    public static T FindChild<T>(GameObject obj, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (obj == null)
        {
            return null;
        }

        // 직계 자식만 탐색
        if (recursive == false)
        {
            for (int i = 0; i < obj.transform.childCount; i++)
            {   
                Transform transform = obj.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                    {
                        return component;
                    }
                }
            }
        }
        else // 직계 자식 하위의 모든 자식까지 탐색
        {
            foreach (T component in obj.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                {
                    return component;
                }
            }
        }

        return null;
    }
}

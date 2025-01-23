using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 싱글톤 패턴(전통적인 방식은 아님)
public class Managers : MonoBehaviour
{
    // 유일성 보장
    private static Managers s_instance;
    public static Managers Instance { get { Initialize(); return s_instance; } } 
    
    void Start()
    {
        Initialize();
    }
    
    void Update()
    {
        
    }

    static void Initialize()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject() { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
        }
    }
}

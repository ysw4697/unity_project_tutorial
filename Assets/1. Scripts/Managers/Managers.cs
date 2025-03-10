using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 싱글톤 패턴(전통적인 방식은 아님)
public class Managers : MonoBehaviour
{
    // 유일성 보장
    private static Managers s_instance;
    public static Managers Instance { get { Init(); return s_instance; } } 
    
    InputManager _input = new InputManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerEx _scene = new SceneManagerEx();
    SoundManager _sound = new SoundManager();
    UIManager _ui = new UIManager();
    
    public static InputManager Input { get { return Instance._input; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UIManager UI { get { return Instance._ui; } }
    
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        _input.OnUpdate();
    }
    
    static void Init()
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

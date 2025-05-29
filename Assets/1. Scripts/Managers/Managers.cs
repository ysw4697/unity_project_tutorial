using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 싱글톤 패턴(전통적인 방식은 아님)
public class Managers : MonoBehaviour
{
    // 유일성 보장
    private static Managers s_instance;
    public static Managers Instance { get { Init(); return s_instance; } }

    #region Contents

    GameManager _game = new GameManager();
    public static GameManager Game { get { return Instance._game; } }

    #endregion

    #region Core

    DataManager _data = new DataManager();
    InputManager _input = new InputManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerEx _scene = new SceneManagerEx();
    SoundManager _sound = new SoundManager();
    UIManager _ui = new UIManager();
    
    public static DataManager Data { get { return Instance._data; } }
    public static InputManager Input { get { return Instance._input; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UIManager UI { get { return Instance._ui; } }
        
    #endregion
    
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
            
            s_instance._data.Init();
            s_instance._pool.Init();
            s_instance._sound.Init();
        }
    }

    // data는 항상 들고 있어야 하기 때문에 Clear를 사용하지 않음
    public static void Clear()
    {
        Input.Clear();
        Scene.Clear();
        Sound.Clear();
        UI.Clear();
        
        Pool.Clear();
    }
}

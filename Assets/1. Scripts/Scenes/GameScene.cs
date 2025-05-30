using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;
        
        //Managers.UI.ShowSceneUI<UI_Inven>();

        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;

        gameObject.GetOrAddComponent<CursorController>();

        GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "Player");
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);
        
        //Managers.Game.Spawn(Define.WorldObject.Monster, "Knight");
        GameObject go = new GameObject { name = "SpawningPool" };
        SpawningPool spawningPool = go.GetOrAddComponent<SpawningPool>();
        spawningPool.SetKeepMonsterCount(5);
    }

    public override void Clear()
    {
        
    }
}
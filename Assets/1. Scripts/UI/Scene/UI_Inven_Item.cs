using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Inven_Item : UI_Base
{
    enum GameObjects
    {
        ItemIcon,
        ItemNameText,
    }

    private string _name;
    
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Get<GameObject>((int)GameObjects.ItemNameText).GetComponent<TMP_Text>().text = _name;
        
        Get<GameObject>((int)GameObjects.ItemIcon).AddUIEvent(data =>
        {
            Debug.Log($"UI_Inven_Item {_name} 클릭");
        });
    }

    public void SetInfo(string name)
    {
        _name = name;
    }
}

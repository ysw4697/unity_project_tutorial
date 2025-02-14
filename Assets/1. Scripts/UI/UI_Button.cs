using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button : UI_Base
{
    enum Buttons
    {
        PointButton,
    }
    
    enum Texts
    {
        PointText,
        ScoreText,
    }

    enum GameObjects
    {
        TestObject,
    }

    private void Start()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        GetText((int)Texts.ScoreText).text = "바인드 테스트";
    }
    
    private int _score = 0;
    
    public void OnButtonClicked()
    {
        _score++;
    }
}

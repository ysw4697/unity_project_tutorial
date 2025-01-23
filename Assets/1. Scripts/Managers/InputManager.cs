using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 리스너 패턴으로 구현
public class InputManager
{
    public Action KeyAction = null;
    
    public void OnUpdate()
    {
        if (Input.anyKey == false)
        {
            return;
        }

        if (KeyAction != null)
        {
            KeyAction.Invoke();
        }
    }
}

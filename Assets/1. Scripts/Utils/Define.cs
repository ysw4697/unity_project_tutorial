using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Scene
    {
        UnKnown,
        Login,
        Lobby,
        Game,
    }
    
    public enum UIEvent
    {
        Click,
        Drag,
    }
    
    public enum MouseEvent
    {
        Pess,
        Click,
    }
    
    public enum CameraMode
    {
        QuaterView,
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    private int _mask = (1 << (int)Define.Layer.Ground | 1 << (int)Define.Layer.Monster);
    
    private Texture2D _attackIcon;
    private Texture2D _handIcon;

    public enum CursorType
    {
        None,
        Attack,
        Hand,
    }
    
    private CursorType _cursorType = CursorType.None;
    
    private void Start()
    {
        _attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursors/Cursor_Attack");
        _handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursors/Cursor_Hand");
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            return;    
        }
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, _mask))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                if (_cursorType != CursorType.Attack)
                {
                    Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 4.67f, _attackIcon.height / 28.0f), 
                        CursorMode.Auto);
                    _cursorType = CursorType.Attack;
                }
            }
            else
            {
                if (_cursorType != CursorType.Hand)
                {
                    Cursor.SetCursor(_handIcon, new Vector2(_handIcon.width / 3.5f, _handIcon.height / 9.3f), 
                        CursorMode.Auto);
                    _cursorType = CursorType.Hand;
                }
            }
        }
    }
}
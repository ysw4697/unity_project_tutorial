using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private PlayerStat _stat;
    private Vector3 _destinationPosition;

    private Texture2D _attackIcon;
    private Texture2D _handIcon;

    public enum CursorType
    {
        None,
        Attack,
        Hand,
    }
    
    private CursorType _cursorType = CursorType.None;
    
    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
        Skill,
    }
    private PlayerState _state = PlayerState.Idle;
    
    private void Start()
    {
        _attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursors/Cursor_Attack");
        _handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursors/Cursor_Hand");
        
        _stat = GetComponent<PlayerStat>();
        
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;
    }
    
    private void Update()
    {
        UpdateMouseCursor();
        
        switch (_state)
        {
            case PlayerState.Die:
                UpdateDie();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Idle:
                UpdateIdle();
                break;
        }
    }

    private void UpdateDie()
    {
        // 아무것도 못함
    }

    private void UpdateMoving()
    {
        Vector3 direction = _destinationPosition - transform.position;
        if (direction.magnitude < 0.1f)
        {
            _state = PlayerState.Idle;
        }
        else
        {
            // todo
            NavMeshAgent navMeshAgent = gameObject.GetOrAddComponent<NavMeshAgent>();
            
            float moveDistance = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0.0f, direction.magnitude);
            navMeshAgent.Move(direction.normalized * moveDistance);

            Debug.DrawRay(transform.position + Vector3.up * 0.5f, direction.normalized, Color.red);
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, direction, 1.0f, LayerMask.GetMask("Block")))
            {
                if (!Input.GetMouseButton(0))
                {
                    _state = PlayerState.Idle;
                }
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 10.0f * Time.deltaTime);
        }
        
        Animator animator = GetComponent<Animator>();
        animator.SetFloat("speed", _stat.MoveSpeed);
    }

    private void UpdateIdle()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetFloat("speed", 0.0f);
    }

    private void UpdateMouseCursor()
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
    
    int _mask = (1 << (int)Define.Layer.Ground | 1 << (int)Define.Layer.Monster);
    private GameObject _lockTarget;
    private void OnMouseEvent(Define.MouseEvent mouseEvent)
    {
        if (_state == PlayerState.Die)
        {
            return;
        }
        
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);
        // Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        switch (mouseEvent)
        {
            case Define.MouseEvent.PointerDown:
            {
                if (raycastHit)
                {
                    _destinationPosition = hit.point;
                    _state = PlayerState.Moving;
                    
                    if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                    {
                        _lockTarget = hit.collider.gameObject;
                    }
                    else
                    {
                        _lockTarget = null;
                    }
                }
            }
                break;
            case Define.MouseEvent.Pess:
            {
                if (_lockTarget != null)
                {
                    _destinationPosition = _lockTarget.transform.position;
                }
                else if (raycastHit)
                {
                    _destinationPosition = hit.point;
                }
            }
                break;
            case Define.MouseEvent.PointerUp:
                _lockTarget = null;
                break;
        }
    }
}

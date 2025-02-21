using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10.0f;
    
    private Vector3 _destinationPosition;
    
    public enum PlayerState
    {
        Die,
        Moving,
        Idle
    }
    private PlayerState _state = PlayerState.Idle;
    
    private void Start()
    {
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;
        
        // 임시
        Managers.UI.ShowSceneUI<UI_Inven>();
    }
    
    private void Update()
    {
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
        if (direction.magnitude < 0.0001f)
        {
            _state = PlayerState.Idle;
        }
        else
        {
            float moveDistance = Mathf.Clamp(_moveSpeed * Time.deltaTime, 0.0f, direction.magnitude);
            transform.position += direction.normalized * moveDistance;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 10.0f * Time.deltaTime);
        }
        
        Animator animator = GetComponent<Animator>();
        animator.SetFloat("speed", _moveSpeed);
    }

    private void UpdateIdle()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetFloat("speed", 0.0f);
    }

    private void OnMouseEvent(Define.MouseEvent mouseEvent)
    {
        if (_state == PlayerState.Die)
        {
            return;
        }
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
            
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Wall")))
        {
            _destinationPosition = hit.point;
            _state = PlayerState.Moving;
        }
    }
}

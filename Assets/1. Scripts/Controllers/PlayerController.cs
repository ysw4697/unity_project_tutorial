using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseController
{
    private int _mask = (1 << (int)Define.Layer.Ground | 1 << (int)Define.Layer.Monster);
    
    private PlayerStat _stat;
    private bool _stopSkill = false;
    
    public override void Init()
    {
        worldObjectType = Define.WorldObject.Player;
        _stat = GetComponent<PlayerStat>();
        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
        {
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
        }
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;
    }
    
    protected override void UpdateMoving()
    {
        // 몬스터가 사정거리보다 가까우면 공격
        if (_lockTarget != null)
        {
            _destinationPosition = _lockTarget.transform.position;
            float distance = (_destinationPosition  - transform.position).magnitude;
            if (distance < 1.5f)
            {
                State = Define.State.Skill;
                return;
            }
        }
        
        // 이동
        Vector3 direction = _destinationPosition - transform.position;
        direction.y = 0.0f;
        
        if (direction.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, direction.normalized, Color.red);
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, direction, 1.0f, LayerMask.GetMask("Block")))
            {
                if (!Input.GetMouseButton(0))
                {
                    State = Define.State.Idle;
                }
                return;
            }

            float moveDistance = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0.0f, direction.magnitude);
            transform.position += direction.normalized * moveDistance;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 10.0f * Time.deltaTime);
        }
    }
    
    protected override void UpdateSkill()
    {
        if (_lockTarget != null)
        {
            Vector3 direction = _lockTarget.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 20.0f * Time.deltaTime);
        }
    }

    private void OnHitEvent()
    {
        if (_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);
        }
        
        if (_stopSkill)
        {
            State = Define.State.Idle;
        }
        else
        {
            State = Define.State.Skill;
        }
    }
    
    private void OnMouseEvent(Define.MouseEvent mouseEvent)
    {
        switch (State)
        {
            case Define.State.Idle:
                OnMouseEvent_IdleRun(mouseEvent);
                break;
            case Define.State.Moving:
                OnMouseEvent_IdleRun(mouseEvent);
                break;
            case Define.State.Skill:
            {
                if (mouseEvent == Define.MouseEvent.PointerUp)
                {
                    _stopSkill = true;
                }
            }
                break;
        }
    }

    private void OnMouseEvent_IdleRun(Define.MouseEvent mouseEvent)
    {
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
                    State = Define.State.Moving;
                    _stopSkill = false;
                    
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
                if (_lockTarget == null && raycastHit)
                {
                    _destinationPosition = hit.point;
                }
            } 
                break;
            case Define.MouseEvent.PointerUp:
                _stopSkill = true;
                break;
        }
    }
}
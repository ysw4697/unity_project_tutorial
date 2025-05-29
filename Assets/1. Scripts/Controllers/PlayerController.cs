using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
        Skill,
    }
    
    private int _mask = (1 << (int)Define.Layer.Ground | 1 << (int)Define.Layer.Monster);
    
    private PlayerStat _stat;
    private Vector3 _destinationPosition;
    
    [SerializeField]
    private PlayerState _state = PlayerState.Idle;

    private GameObject _lockTarget;
    
    public PlayerState State
    {
        get { return _state; }
        set
        {
            _state = value;
            
            Animator animator = GetComponent<Animator>();
            switch (_state)
            {
                case PlayerState.Die:
                    break;
                case PlayerState.Idle:
                    animator.CrossFade("WAIT", 0.1f);
                    break;
                case PlayerState.Moving:
                    animator.CrossFade("RUN", 0.1f);
                    break;
                case PlayerState.Skill:
                    animator.CrossFade("ATTACK", 0.1f, -1, 0);
                    break;
            }
        }
    }
    
    private void Start()
    {
        _stat = GetComponent<PlayerStat>();
        
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }
    
    private void Update()
    {
        switch (State)
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
            case PlayerState.Skill:
                UpdateSkill();
                break;
        }
    }

    private void UpdateDie()
    {
        // 아무것도 못함
    }

    private void UpdateMoving()
    {
        // 몬스터가 사정거리보다 가까우면 공격
        if (_lockTarget != null)
        {
            _destinationPosition = _lockTarget.transform.position;
            float distance = (_destinationPosition  - transform.position).magnitude;
            if (distance < 1.5f)
            {
                State = PlayerState.Skill;
                return;
            }
        }
        
        // 이동
        Vector3 direction = _destinationPosition - transform.position;
        if (direction.magnitude < 0.1f)
        {
            State = PlayerState.Idle;
        }
        else
        {
            NavMeshAgent navMeshAgent = gameObject.GetOrAddComponent<NavMeshAgent>();
            
            float moveDistance = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0.0f, direction.magnitude);
            navMeshAgent.Move(direction.normalized * moveDistance);

            Debug.DrawRay(transform.position + Vector3.up * 0.5f, direction.normalized, Color.red);
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, direction, 1.0f, LayerMask.GetMask("Block")))
            {
                if (!Input.GetMouseButton(0))
                {
                    State = PlayerState.Idle;
                }
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 10.0f * Time.deltaTime);
        }
    }

    private void UpdateIdle()
    {
        
    }

    private void UpdateSkill()
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
            PlayerStat myStat = gameObject.GetComponent<PlayerStat>();
            int damage = Mathf.Max(0, myStat.Attack - targetStat.Defence);
            
            targetStat.Hp -= damage;
        }
        
        if (_stopSkill)
        {
            State = PlayerState.Idle;
        }
        else
        {
            State = PlayerState.Skill;
        }
    }
    
    private bool _stopSkill = false;
    private void OnMouseEvent(Define.MouseEvent mouseEvent)
    {
        switch (State)
        {
            case PlayerState.Idle:
                OnMouseEvent_IdleRun(mouseEvent);
                break;
            case PlayerState.Moving:
                OnMouseEvent_IdleRun(mouseEvent);
                break;
            case PlayerState.Skill:
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
                    State = PlayerState.Moving;
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
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    private Stat _stat;
    
    [SerializeField] private float _scanRange = 10.0f;
    [SerializeField] private float _attackRange = 1.5f;
    
    public override void Init()
    {
        worldObjectType = Define.WorldObject.Monster;
        _stat = GetComponent<Stat>();
        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
        {
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
        }
    }

    protected override void UpdateIdle()
    {
        GameObject player = Managers.Game.GetPlayer();
        if (player == null)
        {
            return;
        }
        
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance <= _scanRange)
        {
            _lockTarget = player;
            State = Define.State.Moving;
            return;
        }
    }

    protected override void UpdateMoving()
    {
        // 몬스터가 사정거리보다 가까우면 공격
        if (_lockTarget != null)
        {
            _destinationPosition = _lockTarget.transform.position;
            float distance = (_destinationPosition  - transform.position).magnitude;
            if (distance < _attackRange)
            {
                NavMeshAgent navMeshAgent = gameObject.GetOrAddComponent<NavMeshAgent>();
                navMeshAgent.SetDestination(transform.position);
                State = Define.State.Skill;
                return;
            }
        }
        
        // 이동
        Vector3 direction = _destinationPosition - transform.position;
        if (direction.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            NavMeshAgent navMeshAgent = gameObject.GetOrAddComponent<NavMeshAgent>();
            navMeshAgent.SetDestination(_destinationPosition);
            navMeshAgent.speed = _stat.MoveSpeed;

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
            
            if (targetStat.Hp > 0)
            {
                float distance = Vector3.Distance(_lockTarget.transform.position, transform.position);
                if (distance <= _attackRange)
                {
                    State = Define.State.Skill;
                }
                else
                {
                    State = Define.State.Moving;
                }
            }
            else
            {
                Managers.Game.Despawn(_lockTarget.gameObject);
                State = Define.State.Idle;
            }
        }
        else
        {
            State = Define.State.Idle;
        }
    }
}

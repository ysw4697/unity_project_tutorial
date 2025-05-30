using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [SerializeField] private int _monsterCount = 0;
    [SerializeField] private int _reserveCount = 0;
    [SerializeField] private int _keepMonsterCount = 0;
    [SerializeField] private Vector3 _spawnPosition = new Vector3(0, 0, 0);
    [SerializeField] private float _spawnRadius = 15.0f;
    [SerializeField] private float _spawnTime = 5.0f;
    
    private void Start()
    {
        Managers.Game.OnSpawnEvent -= AddMonsterCount;
        Managers.Game.OnSpawnEvent += AddMonsterCount;
    }

    private void Update()
    {
        while (_reserveCount + _monsterCount < _keepMonsterCount)
        {
            StartCoroutine("ReserveSpawn");
        }
    }
    
    public void AddMonsterCount(int value) { _monsterCount += value; }
    public void SetKeepMonsterCount(int value) { _keepMonsterCount = value; }

    private IEnumerator ReserveSpawn()
    {
        _reserveCount++;
        
        yield return new WaitForSeconds(Random.Range(0.0f, _spawnTime));
        GameObject obj = Managers.Game.Spawn(Define.WorldObject.Monster, "Knight");
        NavMeshAgent agent = obj.GetOrAddComponent<NavMeshAgent>();

        Vector3 randomPosition;
        while (true)
        {
            Vector3 randomDirection = Random.insideUnitCircle * Random.Range(0.0f, _spawnRadius);
            randomDirection.y = 0.0f;
            randomPosition = _spawnPosition + randomDirection;

            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(randomPosition, path))
            {
                break;
            }
        }
        
        obj.transform.position = randomPosition;
        _reserveCount--;
    }
}

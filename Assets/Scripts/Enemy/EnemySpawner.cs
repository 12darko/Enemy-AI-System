using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private int numberOfEnemiesToSpawn = 5;
    [SerializeField] private float spawnDelay = 1f;
    [SerializeField] private List<Enemy> enemyPrefabs = new List<Enemy>();
    [SerializeField] private EnemiesSpawnMethod enemiesSpawnMethod = EnemiesSpawnMethod.RoundRobin;

    private NavMeshTriangulation _triangulation;
    private Dictionary<int, ObjectPool> _enemyObjectPool = new Dictionary<int, ObjectPool>();


    private void Awake()
    {
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            _enemyObjectPool.Add(i, ObjectPool.CreateInstance(enemyPrefabs[i], numberOfEnemiesToSpawn, Vector3.zero));
        }
    }

    private void Start()
    {
        _triangulation = NavMesh.CalculateTriangulation();

        StartCoroutine(SpawnEnemies());
    }
    
    private IEnumerator SpawnEnemies()
    {
        WaitForSeconds Wait = new WaitForSeconds(spawnDelay);

        int SpawnedEnemies = 0;

        while (SpawnedEnemies < numberOfEnemiesToSpawn)
        {
            if (enemiesSpawnMethod == EnemiesSpawnMethod.RoundRobin)
            {
                SpawnRoundRobinEnemy(SpawnedEnemies);
            }
            else if (enemiesSpawnMethod == EnemiesSpawnMethod.Random)
            {
                SpawnRandomEnemy();
            }

            SpawnedEnemies++;

            yield return Wait;
        }
    }
    
    private void SpawnRoundRobinEnemy(int SpawnedEnemies)
    {
        int SpawnIndex = SpawnedEnemies % enemyPrefabs.Count;

        DoSpawnEnemy(SpawnIndex);
    }

    private void SpawnRandomEnemy()
    {
        DoSpawnEnemy(Random.Range(0, enemyPrefabs.Count));
    }
    
    private void DoSpawnEnemy(int SpawnIndex)
    {
        PoolableObject poolableObject = _enemyObjectPool[SpawnIndex].GetObject();

        if (poolableObject != null)
        {
            Enemy enemy = poolableObject.GetComponent<Enemy>();

            int VertexIndex = Random.Range(0, _triangulation.vertices.Length);

            NavMeshHit Hit;
            if (NavMesh.SamplePosition(_triangulation.vertices[VertexIndex], out Hit, 2f, -1))
            {
                enemy.EnemyAgent.Warp(Hit.position);
                // enemy needs to get enabled and start chasing now.
                enemy.EnemyMovement.StateManager.EnemyData.PlayerTransform = player;
                enemy.EnemyMovement.Triangulation = _triangulation;
                enemy.EnemyAgent.enabled = true; 
                enemy.EnemyMovement.Spawn();
            }
            else
            {
                Debug.LogError($"NavMeshAgent, NavMesh'e yerleştirilemiyor.  {_triangulation.vertices[VertexIndex]}");
            }
        }
        else
        {
            Debug.LogError($"Bilinmeyen enemi tipi {SpawnIndex} nesne havuzunda. Nesnelerin dışında mı?");
        }
    }
}
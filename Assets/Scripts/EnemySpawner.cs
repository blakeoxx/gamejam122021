using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    [SerializeField] private int maxEnemyCount;
    [SerializeField] private float startDelay;
    [SerializeField] private float spawnDelay;
    [SerializeField] private GameObject[] enemyList;
    
    private int currentEnemyCount;
    
    void Awake()
    {
        instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Safety in case there are multiple spawners
        if (instance != this) return;
        
        if (enemyList.Length > 0 && transform.childCount > 0)
        {
            InvokeRepeating(nameof(Spawn), startDelay, spawnDelay);
        }
    }

    void Spawn()
    {
        if (currentEnemyCount >= maxEnemyCount) return;

        // Pick a random enemy type to spawn
        int enemyIdx = Random.Range(0, enemyList.Length);
        GameObject enemyToSpawn = enemyList[enemyIdx];
        
        // Pick a random spawn position
        int spawnerIdx = Random.Range(0, transform.childCount);
        Transform placeToSpawn = transform.GetChild(spawnerIdx);

        Instantiate(enemyToSpawn, placeToSpawn.position, placeToSpawn.rotation);
        currentEnemyCount++;
    }

    public void OnEnemyLeaving()
    {
        currentEnemyCount -= 1;
        if (currentEnemyCount <= 0) currentEnemyCount = 0;
    }
}

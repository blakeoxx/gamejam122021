using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    [SerializeField] private int waveSize;
    [SerializeField] private int maxEnemyCount;
    [SerializeField] private float startDelay;
    [SerializeField] private float spawnDelay;
    [SerializeField] private GameObject[] enemyList;
    
    private int waveEnemyCount;
    private int currentEnemyCount;

    void Awake()
    {
        instance = this;
        waveEnemyCount = 0;
        currentEnemyCount = 0;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // In case there are multiple spawners, only let the primary run our setup logic
        if (instance != this) return;
        
        if (enemyList.Length > 0 && transform.childCount > 0)
        {
            InvokeRepeating(nameof(Spawn), startDelay, spawnDelay);
        }
    }

    void Spawn()
    {
        if (currentEnemyCount >= maxEnemyCount || waveEnemyCount >= waveSize) return;

        // Pick a random enemy type to spawn
        int enemyIdx = Random.Range(0, enemyList.Length);
        GameObject enemyToSpawn = enemyList[enemyIdx];
        
        // Pick a random spawn position
        int spawnerIdx = Random.Range(0, transform.childCount);
        Transform placeToSpawn = transform.GetChild(spawnerIdx);

        Instantiate(enemyToSpawn, placeToSpawn.position, placeToSpawn.rotation);
        waveEnemyCount++;
        currentEnemyCount++;
    }

    // Called by enemies when they're leaving (but not yet destroyed)
    public void OnEnemyLeaving()
    {
        // In case there are multiple spawners, only let the primary run this
        if (instance != this)
        {
            instance.OnEnemyLeaving();
            return;
        }
        
        currentEnemyCount--;
        if (currentEnemyCount <= 0) currentEnemyCount = 0;
    }
}

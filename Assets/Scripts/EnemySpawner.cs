using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    [SerializeField] private GameObject[] enemyList;
    
    private int waveSize;
    private int maxEnemyCount;
    private float spawnDelay;

    private WaveManager waveManager;
    private bool canSpawn;
    private int waveSpawnedCount;
    private int activeEnemyCount;

    void Awake()
    {
        instance = this;
        waveManager = FindObjectOfType<WaveManager>();
        canSpawn = false;
        waveSpawnedCount = 0;
        activeEnemyCount = 0;
    }

    void Update()
    {
        // In case there are multiple spawners, only let the primary run this
        if (instance != this) return;

        if (canSpawn && waveSpawnedCount >= waveSize && activeEnemyCount == 0)
        {
            canSpawn = false;
            waveManager.OnWaveCleared();
        }
    }

    IEnumerator Spawn()
    {
        while (canSpawn && waveSpawnedCount < waveSize)
        {
            if (activeEnemyCount < maxEnemyCount)
            {
                // Pick a random enemy type to spawn
                int enemyIdx = Random.Range(0, enemyList.Length);
                GameObject enemyToSpawn = enemyList[enemyIdx];
        
                // Pick a random spawn position
                int spawnerIdx = Random.Range(0, transform.childCount);
                Transform placeToSpawn = transform.GetChild(spawnerIdx);

                Instantiate(enemyToSpawn, placeToSpawn.position, placeToSpawn.rotation);
                waveSpawnedCount++;
                activeEnemyCount++;
            }
            
            yield return new WaitForSecondsRealtime(spawnDelay);
        }
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
        
        activeEnemyCount--;
        if (activeEnemyCount <= 0) activeEnemyCount = 0;
    }

    public void SetWave(WaveManager.Wave wave)
    {
        waveSize = wave.waveSize;
        maxEnemyCount = wave.maxEnemyCount;
        spawnDelay = wave.spawnDelay;
        waveSpawnedCount = 0;
        activeEnemyCount = 0;
    }
    
    public void SetCanSpawn(bool newCanSpawn)
    {
        if (instance != this)
        {
            instance.SetCanSpawn(newCanSpawn);
            return;
        }
        
        bool oldCanSpawn = canSpawn;
        canSpawn = newCanSpawn;
        if (newCanSpawn && !oldCanSpawn) StartCoroutine(Spawn());
    }

    public int GetWaveEnemyCount()
    {
        return waveSpawnedCount;
    }
}

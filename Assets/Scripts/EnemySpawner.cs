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
        
        if (enemyList.Length < 1 || transform.childCount < 1)
        {
            InvokeRepeating(nameof(Spawn), startDelay, spawnDelay);
        }
    }

    void Spawn()
    {
        if (currentEnemyCount >= maxEnemyCount) return;
        
        // Pick a random enemy type to spawn
        GameObject enemyToSpawn = enemyList[Random.Range(1, enemyList.Length)];
        
        // Pick a random spawn position
        Transform placeToSpawn = transform.GetChild(Random.Range(1, transform.childCount));

        Instantiate(enemyToSpawn, placeToSpawn.position, placeToSpawn.rotation);
    }

    public void OnEnemyLeaving()
    {
        currentEnemyCount -= 1;
        if (currentEnemyCount <= 0) currentEnemyCount = 0;
    }
}

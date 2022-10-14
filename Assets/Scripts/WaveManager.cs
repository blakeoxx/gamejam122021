using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Wave[] waves;
    private int currentWave;
    private bool waveInProgress;
    
    // Start is called before the first frame update
    void Start()
    {
        currentWave = 0;
        waveInProgress = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !waveInProgress && currentWave < waves.Length)
        {
            EnemySpawner.instance.SetWave(waves[currentWave]);
            EnemySpawner.instance.SetCanSpawn(true);
            waveInProgress = true;
        }
    }

    public void OnWaveCleared()
    {
        waveInProgress = false;
        currentWave++;
    }
    
    [Serializable]
    public class Wave
    {
        public int waveSize;
        public int maxEnemyCount;
    }
}
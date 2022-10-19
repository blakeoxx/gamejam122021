using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Text waveTextOverlay;
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
        // Update the overlay text
        if (!waveInProgress)
        {
            if (currentWave < waves.Length)
            {
                waveTextOverlay.text = "Wave " + (currentWave + 1) + "/" + waves.Length + "\n" +
                                       "Press <b>[R]</b> to start";
            }
            else
            {
                waveTextOverlay.text = "You beat the game!" + "\n" +
                                       "Press <b>[R]</b> to restart from wave 1";
            }
        }
        else
        {
            int enemiesLeft = waves[currentWave].waveSize - EnemySpawner.instance.GetWaveEnemyCount();
            waveTextOverlay.text = "Wave " + (currentWave + 1) + "/" + waves.Length + "\n" +
                                   enemiesLeft + " enemies left";
        }
        
        // Start the wave if the start key is pressed
        if (Input.GetKeyDown(KeyCode.R) && !waveInProgress)
        {
            if (currentWave < waves.Length)
            {
                EnemySpawner.instance.SetWave(waves[currentWave]);
                EnemySpawner.instance.SetCanSpawn(true);
                waveInProgress = true;
            }
            else
            {
                currentWave = 0;
                EnemySpawner.instance.SetWave(waves[currentWave]);
                EnemySpawner.instance.SetCanSpawn(true);
                waveInProgress = true;
            }
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
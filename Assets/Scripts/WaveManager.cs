using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Text waveTextOverlay;
    [SerializeField] private Wave[] waves;
    [SerializeField] private float startDelay;
    private int currentWave;
    private float waveStartTimeLeft;
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
                waveTextOverlay.text = "Wave " + (currentWave + 1) + "/" + waves.Length + "\n";
                if (waveStartTimeLeft > 0)
                {
                    string countdown = Math.Round(waveStartTimeLeft, 2).ToString("F1");
                    waveTextOverlay.text += "Starting in " + countdown + " seconds";
                }
                else waveTextOverlay.text += "Press <b>[R]</b> to start";
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
            if (currentWave >= waves.Length)
            {
                currentWave = 0;
            }
            EnemySpawner.instance.SetWave(waves[currentWave]);
            StartCoroutine(StartWave(startDelay, 0.1f));
        }
    }

    public void OnWaveCleared()
    {
        waveInProgress = false;
        currentWave++;
    }

    IEnumerator StartWave(float delay, float delta)
    {
        waveStartTimeLeft = delay + delta;
        while (waveStartTimeLeft > 0)
        {
            waveStartTimeLeft -= delta;
            yield return new WaitForSecondsRealtime(delta);
        }
        waveStartTimeLeft = 0;
        EnemySpawner.instance.SetCanSpawn(true);
        waveInProgress = true;
    }
    
    [Serializable]
    public class Wave
    {
        public int waveSize;
        public int maxEnemyCount;
        public float spawnDelay;
    }
}
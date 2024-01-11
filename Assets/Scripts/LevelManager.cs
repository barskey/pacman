using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    public int currentLevel;
    public GhostMode currentWave {
        get
            {
                return waves[waveIndex];
            }
        private set { }
    }

    public event Action<GhostMode> WaveChanged;

    private float waveTimer;
    private int waveIndex;
    private bool timerPaused;

    private readonly GhostMode[] waves = {
        GhostMode.Scatter,
        GhostMode.Chase,
        GhostMode.Scatter,
        GhostMode.Chase,
        GhostMode.Scatter,
        GhostMode.Chase,
        GhostMode.Scatter,
        GhostMode.Chase,
    };

    // {level 1}, {level 2-4}, {level 5+}
    private float[,] waveTimes =
    {
        { 7, 7, 5 },            // scatter
        { 20, 20, 20 },         // chase
        { 7, 7, 5 },            // scatter
        { 20, 20, 20 },         // chase
        { 5, 5, 5 },            // scatter
        { 20, 1033, 1037 },     // chase
        { 5, 1f/60f, 1f/60f },  // scatter
        { Mathf.Infinity, Mathf.Infinity, Mathf.Infinity } // chase
    };

    private int LevelIndex { get
        {
            if (currentLevel == 1)
            {
                return 0;
            }
            else if (currentLevel > 1 && currentLevel <= 4)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = 1;

        ResetLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (!timerPaused)
        {
            waveTimer += Time.deltaTime;
        }

        if (waveTimer >= waveTimes[waveIndex, LevelIndex])
        {
            ChangeWave();
        }
    }

    private void ResetLevel()
    {
        waveTimer = 0;
        waveIndex = 0;
    }

    private void ChangeWave()
    {
        waveIndex++;
        WaveChanged?.Invoke(waves[waveIndex]);

        waveTimer = 0;
    }
}

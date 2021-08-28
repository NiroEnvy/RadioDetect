using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Transmitter : MonoBehaviour
{
    [SerializeField] private GameObject wavePrefab;
    [SerializeField] private float waveSpawnRateTime =2f;
    [SerializeField] private float waveDeadTime= 5f;
    public float WaveSpeed = 1000000f;
    private float _waveSpawnTimer;
    
    private void Start()
    {
        _waveSpawnTimer = waveSpawnRateTime;
    }

    private void Update()
    {
        if (_waveSpawnTimer <= 0f)
        {
            SpawnWave();
            _waveSpawnTimer = waveSpawnRateTime;
        }

        _waveSpawnTimer -= Time.deltaTime;
    }

    private void SpawnWave()
    {
        var transform1 = transform;
        var wave = Instantiate(wavePrefab, transform1.position, transform1.rotation);
        wave.GetComponent<Wave>().SetSpeed(WaveSpeed);
        Destroy(wave,waveDeadTime);
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private GameObject _powerupContainer;
    [SerializeField]
    private float _timeBetweenEnemySpawns = 5.0f;
    [SerializeField]
    private float _minWaitForPowerup, _maxWaitForPowerup;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (!_stopSpawning)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-8.4f, 8.4f), 8f, 0f), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_timeBetweenEnemySpawns);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (!_stopSpawning)
        {
            int randomPowerup = Random.Range(0, 3);
            GameObject newPowerup = Instantiate(_powerups[randomPowerup], new Vector3(Random.Range(-8.4f, 8.4f), 8f, 0f), Quaternion.identity);
            newPowerup.transform.parent = _powerupContainer.transform;
            yield return new WaitForSeconds(Random.Range(_minWaitForPowerup, _maxWaitForPowerup));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}

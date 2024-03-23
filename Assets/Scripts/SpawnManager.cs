using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _tripleshotPrefab;
    [SerializeField]
    private GameObject _powerupContainer;
    [SerializeField]
    private float _timeBetweenEnemySpawns = 5.0f;
    [SerializeField]
    private float _minWaitForPowerup, _maxWaitForPowerup;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnTripleshotRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-8.4f, 8.4f), 8f, 0f), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_timeBetweenEnemySpawns);
        }
    }

    IEnumerator SpawnTripleshotRoutine()
    {
        while (_stopSpawning == false)
        {
            GameObject newPowerup = Instantiate(_tripleshotPrefab, new Vector3(Random.Range(-8.4f, 8.4f), 8f, 0f), Quaternion.identity);
            newPowerup.transform.parent = _powerupContainer.transform;
            yield return new WaitForSeconds(Random.Range(_minWaitForPowerup, _maxWaitForPowerup));
        }
    }

    public void onPlayerDeath()
    {
        _stopSpawning = true;
    }
}

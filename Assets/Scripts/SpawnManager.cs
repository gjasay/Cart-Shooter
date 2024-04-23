using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject _powerupContainer;
    [SerializeField] private float _timeBetweenEnemySpawns = 3.0f;
    [SerializeField] private float _minWaitForPowerup, _maxWaitForPowerup;
    [SerializeField] private GameObject[] _commonEnemies;
    [SerializeField] private GameObject[] _rareEnemies;
    [SerializeField] private GameObject[] _commonPowerups;
    [SerializeField] private GameObject[] _rarePowerups;

    private bool _stopSpawning = false;
    private int _enemiesAlive = 3;
    private int _enemiesToSpawn = 3;
    private int _wave = 1;
    
    // Start is called before the first frame update
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (!_stopSpawning && _enemiesToSpawn > 0)
        {
            int randomEnemy = Random.Range(1, 11); // 1 - 10
            GameObject newEnemy;

            _enemiesToSpawn--;
            
            if (randomEnemy <= 3)
            {
                Debug.Log("Spawning a rare enemy");
                newEnemy = Instantiate(_rareEnemies[Random.Range(0, _rareEnemies.Length)], new Vector3(Random.Range(-8.4f, 8.4f), 8f, 0f), Quaternion.identity);
            }
            else
            {
                Debug.Log("Spawning a common enemy");
                newEnemy = Instantiate(_commonEnemies[Random.Range(0, _commonEnemies.Length)], new Vector3(Random.Range(-8.4f, 8.4f), 8f, 0f), Quaternion.identity);
            }
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_timeBetweenEnemySpawns);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (!_stopSpawning)
        {
            int randomPowerup = Random.Range(0, 11); // 0 - 10
            GameObject newPowerup;

            if (randomPowerup <= 3)
            {
                Debug.Log("Spawning a rare powerup");
                newPowerup = Instantiate(_rarePowerups[Random.Range(0, _rarePowerups.Length)], new Vector3(Random.Range(-8.4f, 8.4f), 8f, 0f), Quaternion.identity);
            }
            else
            {
                Debug.Log("Spawning a common powerup");
                newPowerup = Instantiate(_commonPowerups[Random.Range(0, _commonPowerups.Length)], new Vector3(Random.Range(-8.4f, 8.4f), 8f, 0f), Quaternion.identity);
            }
            newPowerup.transform.parent = _powerupContainer.transform;
            yield return new WaitForSeconds(Random.Range(_minWaitForPowerup, _maxWaitForPowerup));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public void OnEnemyDeath()
    {
        _enemiesAlive--;
        if (_enemiesAlive <= 0)
        {
            _wave++;
            switch (_wave)
            {
                case 1:
                    Debug.Log("This switch case is being called");
                    _enemiesAlive = 3;
                    break;
                case 2:
                    _enemiesAlive = 6;
                    break;
                case 3:
                    _enemiesAlive = 10;
                    break;
                default:
                    _enemiesAlive = 0;
                    break;
            }
            _enemiesToSpawn = _enemiesAlive;
            StartCoroutine(SpawnEnemyRoutine());
        }

    }
}

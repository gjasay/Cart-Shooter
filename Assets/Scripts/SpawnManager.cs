using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SpawnRoutine");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //spawn game objects every five seconds
    //create a coroutine of type IEnumerator -- Yield Events
    //while (infinite) loop
    //always create infinite loops inside of coroutines
    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-8.4f, 8.4f), 5f, 0f), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }
}

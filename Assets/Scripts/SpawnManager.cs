using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy; 
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
            Instantiate(_enemy, new Vector3(Random.Range(-8.4f, 8.4f), 5f, 0f), Quaternion.identity);
            yield return new WaitForSeconds(5.0f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private bool _isStartingBarrel;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("UI").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
    }

    //check for laser collision (trigger)
    //instantiate explosion at the position of the barrel
    //destroy explostion after 3 seconds

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            if (_explosionPrefab != null)
            {
                Destroy(collision.gameObject);
                if (_uiManager != null && _isStartingBarrel) _uiManager.StartWave(1);
                Explode();
            }
        }
    }

    public void Explode()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}

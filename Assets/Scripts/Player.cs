using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _defaultFireRate = 0.15f;
    [SerializeField]
    private float _tripleShotFireRate = 0.5f;
    private float _canFire = -0.1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _health = 100;
    [SerializeField]
    private float _bulletOffset = 1.5f;
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private float _tripleShotDuration = 5.0f;

    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null )
        {
            Debug.LogError("The Spawn Manager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKey(KeyCode.Space) && Time.time > _canFire)
        {
            FireBullet();
        }

    }
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(_speed * Time.deltaTime * new Vector3(horizontalInput, verticalInput, 0));

        // Restricts bound on Y Axis
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 4.5f), 0);

        // Wrapping on the X axis
        if (transform.position.x > 9.4f)
        {
            transform.position = new Vector3(-9.4f, transform.position.y, 0);
        }
        else if (transform.position.x < -9.4f)
        {
            transform.position = new Vector3(9.4f, transform.position.y, 0);
        }
    }

    void FireBullet()
    {
        _canFire = Time.time + (_isTripleShotActive ? _tripleShotFireRate : _defaultFireRate);

        Vector3 bulletPosition = new Vector3(transform.position.x, transform.position.y + _bulletOffset, 0);

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_bulletPrefab, bulletPosition, Quaternion.identity);
        }
    }

    public void Damage()
    {
        _health -= 25;

        if (_health <= 0)
        {
            _health = 0;
            _lives--;
            if (_lives <= 0)
            {
                _spawnManager.onPlayerDeath();
                Destroy(gameObject);
            }
            else
            {
                _health = 100;
            }
        }
    }

    IEnumerator TripleShotRoutine()
    {
            Debug.Log("Triple Shot Activated!");
            _isTripleShotActive = true;
            yield return new WaitForSeconds(_tripleShotDuration);
            _isTripleShotActive = false;
    }

    public void ActivateTripleShot()
    {
        StartCoroutine(TripleShotRoutine());
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _minSpeed = 3f;
    [SerializeField] private float _maxSpeed = 8f;
    [SerializeField] private GameObject _roverBulletPrefab, _droneBulletPrefab;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] float _minTimeInterval, _maxTimeInterval;
    [SerializeField] private int _enemyId;
    [SerializeField] private float rotationSpeed = 0.75f;

    private float _switchDirectionTimeInterval;
    private float _switchDirectionTime = 0f;
    private bool _isDirectionLeft = false;
    private bool rotateClockwise = true;

    private float _speed;

    private Player _player;
    private Animator _animator;
    private AudioSource _audioSource;
    private SpawnManager _spawnManager;

    private bool _isEnemyAlive = true;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShootRoutine());

        _switchDirectionTimeInterval = Random.Range(_minTimeInterval, _maxTimeInterval);
        _speed = Random.Range(_minSpeed, _maxSpeed);
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        
        if (_player == null )
        {
            Debug.LogError("Player is null");
        }


        if (_animator == null )
        {
            Debug.LogError("Animator is null");
        }

        if (_audioSource == null )
        {
            Debug.LogError("Enemy audio is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {

        switch (_enemyId)
        {
            case 0:
                RoverMovement();
                break;
            case 1:
                DroneMovement();
                break;
        }

        if (_isEnemyAlive)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);

            if (transform.position.y < -6)
            {
                float randomX = Random.Range(-8.5f, 8.5f);
                transform.position = new Vector3(randomX, 7f, 0);
            }
        }
    }

    private void RoverMovement()
    {
        if (_isEnemyAlive && Time.time > _switchDirectionTime)
        {
            if (_isDirectionLeft)
            {
                _isDirectionLeft = false;
            }
            else
            {
                _isDirectionLeft = true;
            }
            _switchDirectionTimeInterval = Random.Range(_minTimeInterval, _maxTimeInterval);
            _switchDirectionTime = Time.time + _switchDirectionTimeInterval;
        }

        if (_isDirectionLeft)
        {
            transform.Translate(Vector3.left * _speed / 2.5f * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * _speed / 2.5f * Time.deltaTime);
        }
    }


    private void DroneMovement()
    {
        if (rotateClockwise)
        {
            transform.Rotate(0, 0, rotationSpeed);
            if (transform.rotation.z >= 0.4)
            {
                rotateClockwise = false;
            }
        }
        else
        {
            transform.Rotate(0, 0, -rotationSpeed);
            if (transform.rotation.z <= -0.4)
            {
                rotateClockwise = true;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                _player.Damage();
                DeathSequence();
                break;
            case "Bullet":
                _player.AddScore(10);
                Destroy(other.gameObject);
                DeathSequence();
                break;
            case "Explosive Bullet":
                _player.AddScore(10);
                Instantiate(_explosionPrefab, other.transform.position, Quaternion.identity);
                DeathSequence();
                break;
            case "Explosion":
                _player.AddScore(10);
                DeathSequence();
                break;
        }

    }

    private void DeathSequence()
    {
        _animator.SetTrigger("OnEnemyDeath");
        _isEnemyAlive = false;
        _audioSource.Play();
        _spawnManager.OnEnemyDeath();
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 1f);

    }

    IEnumerator ShootRoutine()
    {
        while (_isEnemyAlive)
        {
            float secondsInBetween = 0;
            if (_enemyId == 0)
            {
                secondsInBetween = Random.Range(1f, 3f);
            }
            else if (_enemyId == 1)
            {
                secondsInBetween = Random.Range(0.5f, 1f);
            }
            
            yield return new WaitForSeconds(secondsInBetween);
                                                // 0 = Rover, 1 = Drone; I'm switching prefab based on enemyId
            GameObject newBullet = Instantiate(_enemyId == 0 ? _roverBulletPrefab : _droneBulletPrefab, new Vector3(transform.position.x, transform.position.y-1f, transform.position.z), transform.rotation);
            newBullet.GetComponent<Laser>().SetEnemyBullet(_speed * 2f);
        }
    }
}

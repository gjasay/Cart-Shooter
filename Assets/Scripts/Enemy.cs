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
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private float _shootBehindTimeInterval = 1f;

    private float _shootBehindTime = 0f;
    private float _switchDirectionTimeInterval;
    private float _switchDirectionTime = 0f;
    private bool _isDirectionLeft = false;
    private bool rotateClockwise = true;
    private bool _isRamming = false;
    [SerializeField] private float _ramSpeed = 1.25f;
    private float _speed;
    private Vector3 _dodgeDirection;

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
        _shieldVisualizer = transform.GetChild(0).gameObject; // Shield visualizer is the first child of the enemy object
        //Randomly choose a direction to dodge
        _dodgeDirection = Random.Range(0,2) == 0 ? Vector3.right : Vector3.left;
        

        if (_shieldVisualizer == null)
        {
            Debug.LogError("Shield visualizer is null");
        }
        else
        {
            if (Random.Range(0, 11) <= 3)
            {
                _shieldVisualizer.SetActive(true);
            }
            else
            {
                _shieldVisualizer.SetActive(false);
            }
        }

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
        // Enemy specific behavior
        switch (_enemyId)
        {
            case 0:
                CastRayToRam();
                break;
            case 2:
                AvoidShots();
                break;
            case 3:
                DetectAndShootBehind();
                break;
        }
    }

    private void CalculateMovement()
    {

        switch (_enemyId)
        {
            case 1:
                DroneMovement();
                break;
            default:
                RoverMovement();
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
    
    private void CastRayToRam()
    {
        Vector3 raycastOrigin = transform.position + new Vector3(0f, -1f, 0f); // Offset the raycast to be at the bottom of the enemy object (1 unit below the enemy object)
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, transform.TransformDirection(Vector3.down), 2.65f);
        Debug.DrawRay(raycastOrigin, transform.TransformDirection(Vector3.down) * 2.65f, Color.red);

        if (hit && hit.collider.tag == "Player")
        {
            _isRamming = true;
        }
        else
        {
            _isRamming = false;
        }

        if (_isRamming)
        {
            transform.Translate(Vector3.down * _speed * _ramSpeed * Time.deltaTime);
        }
    }

    private void AvoidShots()
    {
        Vector3 raycastOrigin = transform.position + new Vector3(0f, -1f, 0f); // Offset the raycast to be at the bottom of the enemy object (1 unit below the enemy object)
        float raycastDistance = 2.65f;

        // Cast rays 
        RaycastHit2D leftHit = Physics2D.Raycast(raycastOrigin - transform.right * 0.5f, transform.TransformDirection(Vector3.down), raycastDistance);
        RaycastHit2D centerLeftHit = Physics2D.Raycast(raycastOrigin - transform.right * 0.25f, transform.TransformDirection(Vector3.down), raycastDistance);
        RaycastHit2D centerHit = Physics2D.Raycast(raycastOrigin, transform.TransformDirection(Vector3.down), raycastDistance);
        RaycastHit2D centerRightHit = Physics2D.Raycast(raycastOrigin + transform.right * 0.25f, transform.TransformDirection(Vector3.down), raycastDistance);
        RaycastHit2D rightHit = Physics2D.Raycast(raycastOrigin + transform.right * 0.5f, transform.TransformDirection(Vector3.down), raycastDistance);

        // Draw rays for visualization
        Debug.DrawRay(raycastOrigin - transform.right * 0.5f, transform.TransformDirection(Vector3.down) * raycastDistance, Color.red);
        Debug.DrawRay(raycastOrigin - transform.right * 0.25f, transform.TransformDirection(Vector3.down) * raycastDistance, Color.red);
        Debug.DrawRay(raycastOrigin, transform.TransformDirection(Vector3.down) * raycastDistance, Color.red);
        Debug.DrawRay(raycastOrigin + transform.right * 0.25f, transform.TransformDirection(Vector3.down) * raycastDistance, Color.red);
        Debug.DrawRay(raycastOrigin + transform.right * 0.5f, transform.TransformDirection(Vector3.down) * raycastDistance, Color.red);

       if (leftHit && leftHit.collider.tag == "Bullet" 
       || centerLeftHit && centerLeftHit.collider.tag == "Bullet"
       || centerHit && centerHit.collider.tag == "Bullet"
       || centerRightHit && centerRightHit.collider.tag == "Bullet"
       || rightHit && rightHit.collider.tag == "Bullet")
       {
            transform.Translate(_dodgeDirection * _speed * 5 * Time.deltaTime);
       }
    }

    private void DetectAndShootBehind()
    {
        Vector3 raycastOrigin = transform.position + new Vector3(0f, 1f, 0f);
        float raycastDistance = 15f;
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, transform.TransformDirection(Vector3.up), raycastDistance);
        Debug.DrawRay(raycastOrigin, transform.TransformDirection(Vector3.up) * raycastDistance, Color.red);

        if (hit && hit.collider.tag == "Player" && Time.time > _shootBehindTime)
        {
            _shootBehindTime = Time.time + _shootBehindTimeInterval;
            // Shoot behind
            Instantiate(_roverBulletPrefab, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), transform.rotation);
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
        if (_shieldVisualizer.activeSelf)
        {
            _shieldVisualizer.SetActive(false);
        }
        else
        {
            _animator.SetTrigger("OnEnemyDeath");
            _isEnemyAlive = false;
            _audioSource.Play();
            _spawnManager.OnEnemyDeath();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 1f);
        }
    }

    IEnumerator ShootRoutine()
    {
        if (_enemyId != 2)
        {
            while (_isEnemyAlive)
            {
                float secondsInBetween = 0;
                if (_enemyId == 1)
                {
                    
                    secondsInBetween = Random.Range(0.5f, 1f);

                }
                else 
                {
                    secondsInBetween = Random.Range(1f, 3f);
                }

                yield return new WaitForSeconds(secondsInBetween);
                // 0 = Rover, 1 = Drone; I'm switching prefab based on enemyId
                GameObject newBullet = Instantiate(_enemyId == 1 ? _droneBulletPrefab : _roverBulletPrefab, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), transform.rotation);
                newBullet.GetComponent<Laser>().SetEnemyBullet(_speed * 2f);
            }
        }
    }
}

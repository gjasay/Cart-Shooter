using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedMultiplier = 2f;
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
    private int _health = 100;
    [SerializeField]
    private float _bulletOffset = 1.5f;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private int _score;
    [SerializeField]
    private UIManager _uiManager;
    [SerializeField]
    private int _damagePerHit;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("UI").GetComponent<UIManager>();

        if ( _uiManager != null )
        {
            Debug.LogError("The UI Manager is null");
        }

        if (_spawnManager == null )
        {
            Debug.LogError("The Spawn Manager is null");
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

        transform.Translate((_isSpeedBoostActive ? _speedMultiplier * _speed : _speed) * Time.deltaTime * new Vector3(horizontalInput, verticalInput, 0));

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
        if (!_isShieldActive)
        {
            _health -= _damagePerHit;
            _uiManager.UpdateHealth(_health);

            if (_health <= 0)
            {
                _spawnManager.onPlayerDeath();
                Destroy(gameObject);
                _uiManager.GameOver();
            }
        } else
        {
            _isShieldActive = false;

            if (_shieldVisualizer != null)
            {
                _shieldVisualizer.SetActive(false);
            }

            return;
        }
    }

    public IEnumerator TripleShotRoutine(float tripleShotDuration)
    {    
        if (_isTripleShotActive)
        {
            yield return new WaitUntil(() => !_isTripleShotActive);
        }

        _isTripleShotActive = true;
        yield return new WaitForSeconds(tripleShotDuration);
        _isTripleShotActive = false;

    }

    public IEnumerator SpeedBoostRoutine(float speedBoostDuration)
    {
        if (_isSpeedBoostActive)
        {
            yield return new WaitUntil(() => !_isSpeedBoostActive);
        }

        _isSpeedBoostActive = true;
        yield return new WaitForSeconds(speedBoostDuration);
        _isSpeedBoostActive = false;

    }

    public void ActivateShield()
    {
        _isShieldActive = true;
        if (_shieldVisualizer != null)
        {
            _shieldVisualizer.SetActive(true);
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
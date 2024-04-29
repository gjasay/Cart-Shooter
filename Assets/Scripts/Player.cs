using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Movement related variables
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _speedBoostMultiplier = 2f;
    [SerializeField] private float _thrusterMultiplier = 1.5f;
    private bool _isThrusterEnabled = false;
    private float _boostCharge = 100f;
    private float _currentVelocity;
    //Shooting related variables
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _explosiveShotPrefab;
    [SerializeField] private float _defaultFireRate = 0.15f;
    [SerializeField] private float _tripleShotFireRate = 0.5f;
    [SerializeField] private float _bulletOffset = 1.5f;
    [SerializeField] private int _ammo = 15;
    private bool _isTripleShotActive = false;
    private bool _isExplosiveShotActive = false;
    private float _canFire = -0.1f;
    //Health and Shield related variables
    [SerializeField] private int _health = 100;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject _leftEngine, _rightEngine;
    [SerializeField] private int _score;
    [SerializeField] private int _damagePerHit;
    private int _shieldHitsLeft = 0;
    private Color _firstHitShieldTransparency = new Color(255f, 255f, 255f, 0.75f);
    private Color _secondHitShieldTransparency = new Color(255f, 255f, 255f, 0.5f);
    //Misc. Variables
    [SerializeField] private AudioSource _audioSource;
    private bool _isSpeedBoostActive = false;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private GameObject[] _backgrounds;
    private Animator _cameraAnim;



    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        _backgrounds = GameObject.FindGameObjectsWithTag("Background");
        _audioSource = GetComponent<AudioSource>();
        _cameraAnim = GameObject.Find("Main Camera").GetComponent<Animator>();

        if ( _uiManager == null )
        {
            Debug.LogError("The UI Manager is null");
        }
        else
        {
            _uiManager.UpdateHealth(_health);
            _uiManager.UpdateBoost(_boostCharge);
        }

        if (_spawnManager == null )
        {
            Debug.LogError("The Spawn Manager is null");
        }
        if (_audioSource == null )
        {
            Debug.LogError("Player audio source is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        ThrusterInput();
        CalculateMovement();

        if (Input.GetKey(KeyCode.Space) && Time.time > _canFire)
        {
            FireBullet();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            //Find the Powerup object if it exists if it does not exist, return null
            Powerup powerup = GameObject.FindGameObjectWithTag("Powerup") ? GameObject.FindGameObjectWithTag("Powerup").GetComponent<Powerup>() : null;
            
            if (powerup != null) powerup.MoveTowardsPlayer();
        }

    }
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Translate based on whether speed boost is active or not
        transform.Translate((_isSpeedBoostActive ? _speedBoostMultiplier * _speed : _speed) * Time.deltaTime * new Vector3(horizontalInput * 2.5f, verticalInput, 0));

        // Restricts bound on Y Axis
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -5.8f, 4.5f), 0);

        // Wrapping on the X axis
        if (transform.position.x > 9.4f)
        {
            transform.position = new Vector3(-9.4f, transform.position.y, 0);
        }
        else if (transform.position.x < -9.4f)
        {
            transform.position = new Vector3(9.4f, transform.position.y, 0);
        }
        //Handle Player Rotation
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.z, horizontalInput * -15, ref _currentVelocity, 0.25f);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void ThrusterInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ToggleThruster(true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            ToggleThruster(false);
        }

        if (_isThrusterEnabled && _boostCharge <= 0)
        {
            _isThrusterEnabled = false;
            ToggleThruster(false);
        }

        if (_isThrusterEnabled && _boostCharge > 0)
        {
            _boostCharge -= 0.25f;
            _uiManager.UpdateBoost(_boostCharge);
        }
        else if (!_isThrusterEnabled && _boostCharge < 100)
        {
            _boostCharge += 0.1f;
            _uiManager.UpdateBoost(_boostCharge);
        }
    }

    void ToggleThruster(bool setActive)
    {
        _isThrusterEnabled = setActive;
        transform.GetChild(6).gameObject.SetActive(setActive); //Toggle thruster visibilty
        foreach (GameObject background in _backgrounds)
        {
            background.GetComponent<Background>().UpdateSpeed(_thrusterMultiplier, _isThrusterEnabled);
        }
    }

    void FireBullet()
    {
        if (_ammo > 0)
        {
            _ammo--;
            _uiManager.UpdateAmmoCount(_ammo);
            _canFire = Time.time + (_isTripleShotActive ? _tripleShotFireRate : _defaultFireRate);

            Vector3 bulletPosition = new Vector3(transform.position.x, transform.position.y + _bulletOffset, 0);

            if (_isTripleShotActive)
            {
                Instantiate(_tripleShotPrefab, transform.position, transform.rotation);
            }
            else if (_isExplosiveShotActive)
            {
                Instantiate(_explosiveShotPrefab, bulletPosition, transform.rotation);
            }
            else
            {
                Instantiate(_bulletPrefab, bulletPosition, transform.rotation * transform.rotation);
            }

            _audioSource.Play();
        }
    }

    public void Damage()
    {
        _cameraAnim.SetTrigger("OnDamage");
        if (_shieldHitsLeft <= 0)
        {
            _health -= _damagePerHit;
            _uiManager.UpdateHealth(_health);
            UpdateHealthIndicators();
            if (_health <= 0)
            {
                _health = 0;
                _spawnManager.OnPlayerDeath();
                _uiManager.GameOverSequence();
                Destroy(gameObject);
            }
        } 
        else
        {
            _shieldHitsLeft--;

            switch (_shieldHitsLeft)
            {
                case 0:
                    _shieldVisualizer.SetActive(false);
                    break;
                case 1:
                    if (_shieldVisualizer == null) return;
                    _shieldVisualizer.GetComponent<SpriteRenderer>().color = _secondHitShieldTransparency;
                    break;
                case 2:
                    if (_shieldVisualizer == null) return;
                    _shieldVisualizer.GetComponent<SpriteRenderer>().color = _firstHitShieldTransparency;
                    break;
                default:
                    _shieldVisualizer.SetActive(true);
                    break;
            }
        }
        UpdateHealthIndicators();
    }

    void UpdateHealthIndicators()
    {
        if (_health <= 75 && _health > 50)
        {
            _leftEngine.SetActive(true);
        }
        else if (_health <= 50)
        {
            _leftEngine.SetActive(true);
            _rightEngine.SetActive(true);
        }
        else
        {
            _leftEngine.SetActive(false);
            _rightEngine.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy Bullet")
        {
            Destroy(other.gameObject);
            Damage();
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

    public IEnumerator ExplosiveShotRoutine(float explosiveShotDuration)
    {
        if (_isExplosiveShotActive)
        {
            yield return new WaitUntil(() => !_isExplosiveShotActive);
        }

        _isExplosiveShotActive = true;
        yield return new WaitForSeconds(explosiveShotDuration);
        _isExplosiveShotActive = false;
    }

    public void ActivateShield()
    {
        _shieldHitsLeft = 3;
        if (_shieldVisualizer != null)
        {
            _shieldVisualizer.SetActive(true);
            _shieldVisualizer.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void CollectAmmo()
    {
        _ammo += 10;
        _uiManager.UpdateAmmoCount(_ammo);
    }

    public void CollectHealth()
    {
        if (_health <= 85)
        {
            _health += 15;
        }
        else
        {
            _health = 100;
        }
        _uiManager.UpdateHealth(_health);
        UpdateHealthIndicators(); 
    }
}
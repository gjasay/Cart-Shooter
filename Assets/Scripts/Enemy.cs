using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _minSpeed = 3f;
    [SerializeField]
    private float _maxSpeed = 8f;
    [SerializeField]
    private GameObject _bulletPrefab;

    private float _speed;

    private Player _player;
    private Animator _animator;

    private bool _isEnemyAlive = true;

    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShootRoutine());

        _speed = Random.Range(_minSpeed, _maxSpeed);
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        
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

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {

            if (_player != null)
            {
                _player.Damage();
            }
            _animator.SetTrigger("OnEnemyDeath");
            _isEnemyAlive = false;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 1f);
        }
        else if (other.tag == "Bullet")
        {
            if (_player != null)
            {
                _player.AddScore(10);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _isEnemyAlive = false;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(other.gameObject);
            Destroy(this.gameObject, 1f);
        }
    }

    IEnumerator ShootRoutine()
    {
        while (_isEnemyAlive)
        {
            float secondsInBetween = Random.Range(2.0f, 5.0f);
            yield return new WaitForSeconds(secondsInBetween);
            GameObject newBullet = Instantiate(_bulletPrefab, new Vector3(transform.position.x, transform.position.y-1f, transform.position.z), Quaternion.identity);
            newBullet.GetComponent<Laser>().SetEnemyBullet();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _minSpeed = 3f;
    [SerializeField]
    private float _maxSpeed = 8f;

    private float _speed;

    private Player _player;
    private Animator _animator;

    private bool _enemyMoving = true;

    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _speed = Random.Range(_minSpeed, _maxSpeed);
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        
        if (_player == null )
        {
            Debug.LogError("Player is null");
        }

        _animator = GetComponent<Animator>();

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

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {

            if (_player != null)
            {
                _player.Damage();
            }
            _animator.SetTrigger("OnEnemyDeath");
            _enemyMoving = false;
            _audioSource.Play();
            Destroy(this.gameObject, 1f);
        }
        else if (other.tag == "Bullet")
        {
            if (_player != null)
            {
                _player.AddScore(10);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _enemyMoving = false;
            _audioSource.Play();
            Destroy(this.gameObject, 1f);
            Destroy(other.gameObject);
        }

    }

    private void CalculateMovement()
    {
        if (_enemyMoving)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);

            if (transform.position.y < -6)
            {
                float randomX = Random.Range(-8.5f, 8.5f);
                transform.position = new Vector3(randomX, 7f, 0);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private int _healthPoints = 10;
    [SerializeField] private float _shakeDuration = 0.2f;
    [SerializeField] private float _shakeMagnitude = 0.35f;
    [SerializeField] private float _redDuration = 0.25f;
    [SerializeField] private float _bulletSpeed = 10f;
    [SerializeField] private float _secondsBetweenBullets = 0.75f;

    private bool _isBossActive = false;
    private bool _isShootRoutineActive = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 9, 0);
    }

    // Update is called once per frame
    void Update()
    {
        MoveToMiddle();
        if (_isBossActive && !_isShootRoutineActive)
        {
            StartCoroutine(ShootRoutine());
            return;
        }
    }

    private void MoveToMiddle()
    {
        if (transform.position.y > 0)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        else if (transform.position.y <= 0)
        {
            _isBossActive = true;
            return;
        }
    }

    IEnumerator ShootRoutine()
    {
        Vector3 bulletPosition = new Vector3(0, 2, 0);
        _isShootRoutineActive = true;
        while (_isBossActive)
        {
            GameObject bullet = Instantiate(_laserPrefab, bulletPosition, Quaternion.identity);
            bullet.GetComponent<Laser>().SetBossBullet(_bulletSpeed);
            bullet.transform.parent = this.transform;
            yield return new WaitForSeconds(_secondsBetweenBullets);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy Bullet")
        {
            Destroy(collision.gameObject);
            Damage();
        }
    }

    private void Damage()
    {

        if (_healthPoints <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            _healthPoints--;
            StartCoroutine(ShakeAndFlashRed());
        }
        
    }

    IEnumerator ShakeAndFlashRed()
    {
        Vector3 originalPosition = transform.position;

        Color originalColor = GetComponent<SpriteRenderer>().color;

        for (float i = 0; i < _shakeDuration; i += Time.deltaTime)
        {
            transform.position = originalPosition + Random.insideUnitSphere * _shakeMagnitude;
            yield return null;
        }

        transform.position = originalPosition;

        for (float i = 0; i < _redDuration; i += Time.deltaTime)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            yield return null;
        }

        GetComponent<SpriteRenderer>().color = originalColor;
    }
}

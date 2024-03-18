﻿using System.Collections;
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
    private float _fireRate = 0.5f;
    private float _canFire = -0.1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _health = 100;

    // Start is called before the first frame update
    void Start()
    {
        // Our starting position         x, y, z
        transform.position = new Vector3(0, -2, 0);
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
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.5f, 0));

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
        _canFire = Time.time + _fireRate;
        Vector3 bulletPosition = new Vector3(transform.position.x, transform.position.y + 0.7f, 0);
        Instantiate(_bulletPrefab, bulletPosition, Quaternion.identity);
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
                Destroy(gameObject);
            }
            else
            {
                _health = 100;
            }
        }
    }
}
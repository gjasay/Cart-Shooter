using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;

    private bool _isEnemyBullet = false;

    // Update is called once per frame
    void Update()
    {
       CalculateMovement();
    }

    private void CalculateMovement()
    {
        if (_isEnemyBullet)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);

            if (transform.position.y < -8)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);

            if (transform.position.y > 8)
            {
                
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }

                Destroy(gameObject);
            }
        }
    }
    public void SetEnemyBullet()
    {
        _isEnemyBullet = true;
    }
}

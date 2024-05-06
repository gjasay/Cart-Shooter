using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 8.0f;

    private bool _isEnemyBullet = false;
    private bool _isBossBullet = false;
    private bool _isHoming = false;

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
        else if (_isHoming)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }

            if (closestEnemy != null)
            {
                Vector3 direction = (closestEnemy.transform.position - transform.position).normalized;

                // Calculate angle
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                angle -= 90f;

                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

                // Rotate towards the enemy
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime);
                transform.Translate(Vector3.up * _speed * Time.deltaTime);
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
        else if (_isBossBullet)
        {
            BossBulletMovement();
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

    private void BossBulletMovement()
    {
        GameObject player = GameObject.Find("Player");

        if (player != null)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;

            // Calculate angle
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            angle -= 90f;

            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Rotate towards the enemy
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 180f * Time.deltaTime);
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
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

    public void SetEnemyBullet(float speed)
    {
        _speed = speed;
        _isEnemyBullet = true;
    }

    public void SetHoming()
    {
        _isHoming = true;
    }

    public void SetBossBullet(float speed)
    {
        _speed = speed;
        _isBossBullet = true;
    }
}

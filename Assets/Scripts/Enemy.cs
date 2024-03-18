using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move down at 4 meters per second

        //if bottom of screen
        //respawn at top with a new random x position

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y  < -5)
        {
            float randomX = Random.Range(-8.5f, 8.5f);
            transform.position = new Vector3(randomX, 5.3f, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // if other is player
        // damage player
        // self destruct

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            Destroy(this.gameObject);
        }

        // if other is laser
        // destroy laser
        // self destruct

        else if (other.tag == "Bullet")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }

    }
}

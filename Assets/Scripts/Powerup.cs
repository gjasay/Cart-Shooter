using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
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
        //move down at speed
        //when we leave screen => destroy

        transform.Translate(Vector3.down * _speed *  Time.deltaTime);
        if (transform.position.y < -5f)
        {
            Destroy(this.gameObject);
        }
    }
    //OnTrigger
    //Only collect by player
    //On collect => destroy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();

            if (player != null)
            {
                player.ActivateTripleShot();
            }
            else
            {
                Debug.Log("Player Component is NULL");
            }

            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f; //IDs for powerups: TripleShot: 0, Speed Boost: 1, Shields: 2
    [SerializeField]
    private float _tripleShotDuration = 5.0f;
    [SerializeField]
    private float _speedBoostDuration = 5.0f;
    [SerializeField]
    private float _shieldDuration = 5.0f;
    [SerializeField]
    private int _powerupID;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed *  Time.deltaTime);
        if (transform.position.y < -5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();

            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.StartCoroutine(player.TripleShotRoutine(_tripleShotDuration));
                        break;
                    case 1:
                        player.StartCoroutine(player.SpeedBoostRoutine(_speedBoostDuration));

                        //Finds both background objects and starts the Coroutine for both
                        GameObject[] backgrounds = GameObject.FindGameObjectsWithTag("Background");

                        foreach (GameObject background in backgrounds )
                        {
                            background.GetComponent<Background>().ActivateSpeedBoost(_speedBoostDuration);
                        }

                        break;
                    case 2: 
                        Debug.Log("Shields Activated!");
                        break;
                    default:
                        Debug.Log("Invalid Powerup ID");
                        break;
                }
            }
            else
            {
                Debug.Log("Player Component is NULL");
            }

            Destroy(this.gameObject);
        }
    }
}

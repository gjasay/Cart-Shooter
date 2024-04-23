using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f; //IDs for powerups: TripleShot: 0, Speed Boost: 1, Shields: 2
    [SerializeField] private float _tripleShotDuration = 5.0f;
    [SerializeField] private float _speedBoostDuration = 5.0f;
    [SerializeField] private float _explosiveShotDuration = 5.0f;
    [SerializeField] private int _powerupID;
    [SerializeField] private AudioClip _soundEffect;

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

            AudioSource.PlayClipAtPoint(_soundEffect, transform.position);

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
                        player.ActivateShield();
                        break;
                    case 3:
                        player.CollectAmmo();
                        break;
                    case 4:
                        player.CollectHealth();
                        break;
                    case 5:
                        player.StartCoroutine(player.ExplosiveShotRoutine(_explosiveShotDuration));
                        break;
                    case 6:
                        player.Damage();
                        GetComponent<Barrel>().Explode();
                        break;
                    default:
                        Debug.LogError("Invalid Powerup ID");
                        break;
                }
            }
            else
            {
                Debug.LogError("Player Component is null");
            }

            if (_powerupID != 6) Destroy(this.gameObject);
        }
    }
}

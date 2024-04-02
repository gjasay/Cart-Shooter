using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null )
        {
            Debug.LogError("Explosion audio source is null.");
        }
        else
        {
            _audioSource.Play();
        }
        Destroy(gameObject, 1.25f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackground : MonoBehaviour
{
    private bool _isMovingDown = true;
    private float _speed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_isMovingDown)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            transform.Translate(Vector3.right * _speed/2 * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
            transform.Translate(Vector3.left * _speed/2 * Time.deltaTime);
        }

        if (transform.position.y <= -1)
        {
            _isMovingDown=false;
        }
        else if (transform.position.y >= 1)
        {
            _isMovingDown = true;
        }
    }
}

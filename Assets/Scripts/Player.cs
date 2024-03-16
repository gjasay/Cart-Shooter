using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    [SerializeField]
    private GameObject _bulletPrefab;

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

        // If I hit the space key
        // spawn the gameobject

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(_speed * Time.deltaTime * new Vector3(horizontalInput, verticalInput, 0));

        // Restricts bound on Y Axis
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.5f, 0));

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
}

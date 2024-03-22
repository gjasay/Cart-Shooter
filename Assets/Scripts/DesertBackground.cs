using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertBackground : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private bool _createdBackground = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed *  Time.deltaTime);

        if (transform.position.y < -6.8f && _createdBackground == false)
        {
            _createdBackground = true;
            Vector3 startPos = new Vector3(transform.position.x, 28.7f, 0);
            GameObject newDesert = Instantiate(gameObject, startPos, Quaternion.identity);
            newDesert.transform.name = "Background";
        }
        else if (transform.position.y < -25)
        {
            Destroy(gameObject);
        }
    }
}

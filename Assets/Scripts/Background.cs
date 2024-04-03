using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private float _speedMultiplier = 2.0f;

    private bool _isSpeedBoostActive = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();                        
    }

    IEnumerator SpeedBoostRoutine(float speedBoostDuration)
    {
        if (_isSpeedBoostActive)
        {
            yield return new WaitUntil(() => !_isSpeedBoostActive);
        }

        _isSpeedBoostActive = true;
        UpdateSpeed();
        yield return new WaitForSeconds(speedBoostDuration);
        _isSpeedBoostActive = false;
        UpdateSpeed();
    }

    public void ActivateSpeedBoost(float speedBoostDuration)
    {
        StartCoroutine(SpeedBoostRoutine(speedBoostDuration));
    }

    void UpdateSpeed()
    {
        if (_isSpeedBoostActive)
        {
            _speed *= _speedMultiplier;
        }
        else
        {
            _speed /= _speedMultiplier;
            
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -30f)
        {
            transform.position = new Vector3(transform.position.x, 35.5f, 0);
        }
    }
}

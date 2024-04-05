using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private GameObject _otherBackground;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CalculateMovement();                        
    }

    IEnumerator SpeedBoostRoutine(float speedBoostDuration, float speedMultiplier)
    {
        if (_isSpeedBoostActive)
        {
            yield return new WaitUntil(() => !_isSpeedBoostActive);
        }

        _isSpeedBoostActive = true;
        UpdateSpeed(speedMultiplier, _isSpeedBoostActive);
        yield return new WaitForSeconds(speedBoostDuration);
        _isSpeedBoostActive = false;
        UpdateSpeed(speedMultiplier, _isSpeedBoostActive);
    }

    public void ActivateSpeedBoost(float speedBoostDuration, float speedMultiplier = 2.0f)
    {
        StartCoroutine(SpeedBoostRoutine(speedBoostDuration, speedMultiplier));
    }

    public void UpdateSpeed(float speedMultiplier, bool isActive)
    {
        if (isActive)
        {
            _speed *= speedMultiplier;
        }
        else
        {
            _speed /= speedMultiplier;
            
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.fixedDeltaTime);

        if (transform.position.y <= -25f)
        {
            float backgroundOffset = _otherBackground.transform.position.y + _otherBackground.GetComponent<SpriteRenderer>().bounds.size.y;
            transform.position = new Vector3(transform.position.x, backgroundOffset, 0);
        }
        else if (_otherBackground.transform.position.y > transform.position.y)
        {
            float backgroundOffset = _otherBackground.transform.position.y - _otherBackground.GetComponent<SpriteRenderer>().bounds.size.y;
            transform.position = new Vector3(transform.position.x, backgroundOffset, 0);
        }
    }

}

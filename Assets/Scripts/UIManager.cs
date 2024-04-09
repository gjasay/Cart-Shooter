using MagicPigGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _ammoText;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private GameObject _restartText;
    [SerializeField] private GameObject _gameOverDisplay;
    [SerializeField] private float _flickerInterval = 0.5f;
    private GameManager _gameManager;
    private ProgressBar _healthBar;
    private ProgressBar _boostBar;
    // Start is called before the first frame update
    void Start()
    {
        _gameOverDisplay.SetActive(false);
        _healthBar = GameObject.Find("Health Bar").GetComponent<ProgressBar>();
        _boostBar = GameObject.Find("Boost Bar").GetComponent<ProgressBar>();

        _healthBar.SetProgress(1); //Sets UI to show full health

        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateAmmoCount(int ammo)
    {
        _ammoText.text = "Ammo: " + ammo.ToString();
    }

    public void UpdateHealth(int health)
    {
        float progress = health / 100f;

        if (progress < 0)
        {
            progress = 0;
        }
        else if (progress > 1)
        {
            progress = 1;
        }

        _healthBar.SetProgress(progress);
    }

    public void UpdateBoost(float boost)
    {
        float progress = boost / 100f;

        if (progress < 0)
        {
            progress = 0;
        }
        else if (progress > 1)
        {
            progress = 1;
        }

        _boostBar.SetProgress(progress);
    }

    IEnumerator GameOverFlickerRoutine()
    {
        _gameOverText.text = "";

        string gameOverText = "Game Over";
        for (int i = 0; i <= gameOverText.Length; i++)
        {
            _gameOverText.text = gameOverText.Substring(0, i);
            yield return new WaitForSeconds(_flickerInterval);
        }

        while (true)
        {
            _gameOverText.text = "";
            yield return new WaitForSeconds(_flickerInterval);
            _gameOverText.text = gameOverText;
            yield return new WaitForSeconds(_flickerInterval);
        }

    }

    public void GameOverSequence()
    {
        _gameOverDisplay.SetActive(true);
        _restartText.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
        _gameManager.GameOver();
    }
}

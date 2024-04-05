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
    private ProgressBar _progressBar;
    // Start is called before the first frame update
    void Start()
    {
        _gameOverDisplay.SetActive(false);
        _progressBar = GameObject.Find("Progress Bar").GetComponent<ProgressBar>();
        _progressBar.SetProgress(1); //Sets UI to show full health

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
        _progressBar.SetProgress(progress);
    }

    IEnumerator GameOverFlickerRoutine()
    {
        _gameOverText.text = "G";
        yield return new WaitForSeconds(_flickerInterval);
        _gameOverText.text = "Ga";
        yield return new WaitForSeconds(_flickerInterval);
        _gameOverText.text = "Gam";
        yield return new WaitForSeconds(_flickerInterval);
        _gameOverText.text = "Game";
        yield return new WaitForSeconds(_flickerInterval);
        _gameOverText.text = "Game O";
        yield return new WaitForSeconds(_flickerInterval);
        _gameOverText.text = "Game Ov";
        yield return new WaitForSeconds(_flickerInterval);
        _gameOverText.text = "Game Ove";
        yield return new WaitForSeconds(_flickerInterval);
        _gameOverText.text = "Game Over";
        yield return new WaitForSeconds(_flickerInterval);
        
        while (true)
        {
            _gameOverText.text = "";
            yield return new WaitForSeconds(_flickerInterval);
            _gameOverText.text = "Game Over";
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

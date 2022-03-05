using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _LiveImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _GameOverText;
    [SerializeField]
    private Text _restartText;
    // Start is called before the first frame update
    void Start()
    {
        
        _scoreText.text = "Score " + 0;
        _GameOverText.gameObject.SetActive(false);
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _LiveImg.sprite = _liveSprites[currentLives];

        if(currentLives == 0)
        {
            GameOverSequence();
        }

    }

    void GameOverSequence()
    {
        _GameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true); 
        StartCoroutine(GameOverFlickerRoutine());

    }
    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            //game over flickiring
            _GameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _GameOverText.text= " ";
            yield return new WaitForSeconds(0.5f);
        }
    }
}

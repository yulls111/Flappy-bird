using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Player player;

    public Text scoreText;
    public Text countdownText;

    public GameObject playButton;

    public GameObject game;

    public GameObject over;

    public Parallax[] parallaxElements;
    public Spowner spawner;
    public float difficultyIncreaseInterval = 10f;
    public float speedIncreaseFactor = 0.1f;
    public float spawnRateDecreaseFactor = 0.05f;

    private int score;
    private float nextDifficultyIncreaseTime;


    private void Awake() 
    {
        Application.targetFrameRate = 60;
        Pause();
        countdownText.gameObject.SetActive(false); // —крываем текст отсчета при старте
        over.SetActive(false); // —крываем Game Over при старте
    }

    public void Play() 
    {
        spawner.ClearAllPipes();
        score = 0;

        scoreText.text = score.ToString();
        Pipes.speedMultiplier = 1f;

        playButton.SetActive(false);
        game.SetActive(false);
        over.SetActive(false);

        player.enabled = false; // ќтключаем управление игроком во врем€ отсчета


        //Time.timeScale = 1f;

        //player.enabled = true;

        foreach (var parallax in parallaxElements)
        {
            parallax.animationSpeed = 1f;
        }
        spawner.spawnRate = 1f;
        nextDifficultyIncreaseTime = Time.time + difficultyIncreaseInterval;
        

        // —брос игрока
        player.transform.position = Vector3.zero;
        player.ResetPlayer();

        StartCoroutine(Countdown());


        //-Pipes pipe = FindFirstObjectByType<Pipes>();
        //if (pipe != null)
        //{
        //  Destroy(pipe.gameObject);
        //}

        //-Pipes[] pipes = FindObjectOfType<Pipes>();
        //for (int i = 0; i < pipes.Length; i++) {
        //  Destroy(pipes[i].gameObject); 
        //}

    }
    private IEnumerator Countdown()
    {
        countdownText.gameObject.SetActive(true);

        countdownText.text = "3";
        yield return new WaitForSecondsRealtime(1f);

        countdownText.text = "2";
        yield return new WaitForSecondsRealtime(1f);

        countdownText.text = "1";
        yield return new WaitForSecondsRealtime(1f);

        countdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(0.5f);

        countdownText.gameObject.SetActive(false);

        // ¬ключаем управление и запускаем игру
        player.enabled = true;
        Time.timeScale = 1f;
    }

    public void Pause() 
    {
        Time.timeScale = 0f;
        player.enabled = false;
    }


    public void GameOver() 
    {
        game.SetActive(true);
        over.SetActive(true);
        playButton.SetActive(true);

        Pause();
    
    }

    public void IncreaseScore() 
    {
        score++;
        scoreText.text = score.ToString();

        if (Time.time >= nextDifficultyIncreaseTime)
        {
            IncreaseDifficulty();
            nextDifficultyIncreaseTime = Time.time + difficultyIncreaseInterval;
        }
    }

    private void IncreaseDifficulty()
    {
        foreach (var parallax in parallaxElements)
        {
            parallax.animationSpeed *= (1f + speedIncreaseFactor);
        }

        spawner.spawnRate = Mathf.Max(0.5f, spawner.spawnRate * (1f - spawnRateDecreaseFactor));

        Pipes.speedMultiplier *= (1f + speedIncreaseFactor);
    }

    
}

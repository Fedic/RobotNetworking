using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameUI gameUI;
    private GameObject player;
    public int score;
    public int waveCountdown;
    public bool isGameOver;
    private static Game singleton;
    

    [SerializeField]
    RobotSpawn[] spawns;

    public int enemiesLeft;

    // Start is called before the first frame update
    void Start()
    {
        singleton = this;
        StartCoroutine("increaseScoreEachSecond");
        isGameOver = false;
        Time.timeScale = 1;
        waveCountdown = 30;
        enemiesLeft = 0;
        StartCoroutine("updateWaveTimer");
        SpawnRobots();
        
    }

    public void AssignPlayer(GameObject instantiatedPlayer)
    {
        // make sure instantiatedPlayer is not null
        if (instantiatedPlayer != null)
        {
            // get the Player component from the instantiated player GameObject
            Player playerComponent = instantiatedPlayer.GetComponent<Player>();

            // ensure the Player component is found
            if (playerComponent != null)
            {
                // set the player field in the Game script
                player = instantiatedPlayer;

                // pass the Player component to the SetPlayer method of the game UI
                gameUI.SetPlayer(playerComponent);
            }
            else
            {
                Debug.LogError("The instantiated player GameObject does not have a Player component.");
            }
        }
        else
        {
            Debug.LogError("The instantiated player GameObject is null.");
        }
    }


    private void SpawnRobots()
    {
        foreach(RobotSpawn spawn in spawns)
        {
            spawn.SpawnRobot();
            enemiesLeft++;
        }
        gameUI.SetEnemyText(enemiesLeft);
    }

    private IEnumerator updateWaveTimer()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(1f);
            waveCountdown--;
            gameUI.SetWaveText(waveCountdown);

            if (waveCountdown == 0)
            {
                SpawnRobots();
                waveCountdown = 30;
                gameUI.ShowNewWaveText();
            }
        }
    }

    public static void RemoveEnemy()
    {
        singleton.enemiesLeft--;
        singleton.gameUI.SetEnemyText(singleton.enemiesLeft);

        if(singleton.enemiesLeft == 0)
        {
            singleton.score += 50;
            singleton.gameUI.ShowWaveClearBonus();
        }
    }

    public void AddRobotKillToScore()
    {
        score += 10;
        gameUI.SetScoreText(score);
    }

    IEnumerator increaseScoreEachSecond()
    {
        while(!isGameOver)
        {
            yield return new WaitForSeconds(1);
            score += 1;
            gameUI.SetScoreText(score);
        }
    }

    public void OnGUI()
    {
        if(isGameOver && Cursor.visible == false)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;
        player.GetComponent<FirstPersonController>().enabled = false;
        player.GetComponent<CharacterController>().enabled = false;
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(Constants.SceneBattle);
        gameOverPanel.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(Constants.SceneMenu);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

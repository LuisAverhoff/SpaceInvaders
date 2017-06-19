using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private EnemyController enemyController;

    public GameObject[] bunkers;
    public Transform bunkerSpawnPoint;
    public float bunkerSpawnDistance;

    private AudioSource gameControllerSoundSource;
    public AudioClip [] soundEffectClips;

    public Text playerScoreText;
    public Text highScoreText;
    private int currentPlayerScore;
    private int currentHighScore;

    public Text playerBeatsWaveText;
    private readonly string playerBeatsWaveMessage = "Great job, get ready for the next wave.";

    public Text gameOverText;
    private readonly string gameOverMessage = "GAME OVER!\n\nThe Aliens have won.";

    private readonly string mainMenuScene = "MainMenu";

    private readonly int MAX_PLAYER_LIVES = 5;
    public int startingPlayerLives;
    private int totalPlayerLives;

    private readonly int MAXIMUM_EXTRA_LIVES_ALLOWED = 2;
    private readonly int[] SCORES_FOR_EXTRA_LIVES = {5000, 10000};
    private int extraScoreIndex;
    private int amountOfExtraLivesGiven;

    public Image[] playerLiveImages;

    // Use this for initialization
    void Start()
    {
        enemyController = GameObject.FindWithTag("EnemyController").GetComponent<EnemyController>();
        gameControllerSoundSource = GetComponent<AudioSource>();
        totalPlayerLives = startingPlayerLives;
        amountOfExtraLivesGiven = 0;
        extraScoreIndex = 0;
        currentPlayerScore = 0;
        currentHighScore = PlayerPrefs.GetInt("High Score", 0);
        updateScore();
        updateHighScore();
        playerBeatsWaveText.text = "";
        gameOverText.text = "";
        initializePlayerLifeBar();
        createBunkers();
    }

    public void startNextWave()
    {
        playerBeatsWaveText.text = playerBeatsWaveMessage;
        gameControllerSoundSource.PlayOneShot(soundEffectClips[0]);
        Invoke("restartLevel", soundEffectClips[0].length);
    }

    public void playGameOverScene()
    {
        gameOverText.text = gameOverMessage;
        gameControllerSoundSource.PlayOneShot(soundEffectClips[1]);
        Invoke("goToMainMenu", soundEffectClips[1].length);
    }

    private void createBunkers()
    {
        Vector3 bunkerStartingPosition = bunkerSpawnPoint.position;

        for(int i = 0; i < bunkers.Length; i++)
        {
            Instantiate(bunkers[i], bunkerStartingPosition, Quaternion.identity);
            bunkerStartingPosition.x += bunkerSpawnDistance;
        }
    }

    public void addScore(int points)
    {
        currentPlayerScore += points;
        updateScore();
        setNewHighScore();

        if (amountOfExtraLivesGiven != MAXIMUM_EXTRA_LIVES_ALLOWED)
        {
            if(currentPlayerScore >= SCORES_FOR_EXTRA_LIVES[extraScoreIndex])
            {
                giveExtraLife();
                gameControllerSoundSource.PlayOneShot(soundEffectClips[2]);
                extraScoreIndex++;
                amountOfExtraLivesGiven++;
            }
        }
    }

    private void updateScore()
    {
        playerScoreText.text = "Score: " + "< " + currentPlayerScore.ToString() + " >";
    }

    private void updateHighScore()
    {
        highScoreText.text = "HighScore: " + "< " + currentHighScore.ToString() + " >";
    }

    private void setNewHighScore()
    {
        if(currentPlayerScore > currentHighScore)
        {
            currentHighScore = currentPlayerScore;
            updateHighScore();
            PlayerPrefs.SetInt("High Score", currentHighScore);
            PlayerPrefs.Save();
        }
    }

    private void initializePlayerLifeBar()
    {
        for (int i = 0; i < MAX_PLAYER_LIVES; i++)
        {
            if (totalPlayerLives <= i)
            {
                playerLiveImages[i].enabled = false;
            }
            else
                playerLiveImages[i].enabled = true;
        }
    }

    private void giveExtraLife()
    {
        playerLiveImages[totalPlayerLives].enabled = true;
        totalPlayerLives++;
    }

    public void decrementPlayerLives()
    {
        totalPlayerLives--;
        playerLiveImages[totalPlayerLives].enabled = false;
    }

    public bool doesPlayerHaveLivesLeft()
    {
        return totalPlayerLives != 0;
    }

    public int getTotalPlayerLives()
    {
        return totalPlayerLives;
    }

    private void restartLevel()
    {
        playerBeatsWaveText.text = "";
        enemyController.setEnemiesToInitialState();
        enemyController.spawnEnemies();
    }

    private void goToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}

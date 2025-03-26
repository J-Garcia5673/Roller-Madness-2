using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using Unity.Mathematics;
using JetBrains.Annotations;



public class GameManager : MonoBehaviour
{
  public static GameManager gm;
  public GameObject Player;
  [HideInInspector]
  public int score = 0;
  public int beatLevelScore;
  public int rampScore;
  public int playerHealth = 3;
  public bool canBeatLevel = false;

  public GameObject mainCanvas;
  public TMP_Text scoreText;
  public TMP_Text healthText;
  public GameObject rampText;
  public GameObject winText;
  public GameObject GameOverText;
  public Button PlayAgainButton;
  public Button NextLevelButton;
  public Button ResetGameButton;

  public AudioClip backgrounMusic;
  public AudioClip gameOverSFX;
  public AudioClip beatLevelSFX;
  public GameObject pauseCanvas;
  public Vector3 pointA = new Vector3(0, 0, 0);
  public Vector3 pointB = new Vector3(5, 0, 0);
  public Vector3 pointC = new Vector3(5, 0, 0);
  public float speed = 2.0f;
  public GameObject CoinPrefab;
  public GameObject EnemyPrefab;
  public GameObject[] spawnPoint;
  public GameObject ramp1;
  public GameObject ramp2;

  public float maxX = 23f;
  public float maxZ = 23f;

  Vector3 playerSpawnLocation;
  GameObject cam;
  AudioSource audioSource;
  float originalSpeed = 8;

  public bool pauseGame = false;
  public float rotationAngle = 30.0f;






  void Awake()
  {
    if (gm == null) gm = this;
  }
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    cam = GameObject.FindGameObjectWithTag("MainCamera");
    playAudioRepeat(backgrounMusic);
    if (Player == null)
    {
      Player = GameObject.FindWithTag("Player");
    }
    if (Player == null)
    {
      Debug.LogError("Player not found in the Game");
    }
    setupDefaults();
    PlayAgainButton.onClick.AddListener(playAgain);
    NextLevelButton.onClick.AddListener(nextLevel);
    ResetGameButton.onClick.AddListener(resetGame);

    InvokeRepeating("SpawnCoin", 1f, 3f);
    InvokeRepeating("SpawnEnemy", 4f, 4f);

  }

  // Update is called once per frame
  void Update()
  {




  }
  void setupDefaults()
  {
    GameOverText.SetActive(false);
    winText.SetActive(false);
    PlayAgainButton.gameObject.SetActive(false);
    ResetGameButton.gameObject.SetActive(false);
    rampText.SetActive(false);
    NextLevelButton.gameObject.SetActive(false);
    rampText.SetActive(false);
    ramp1.SetActive(false);
    ramp2.SetActive(false);
    score = 0;
    playerHealth = 3;


    //PauseCanvas.SetActive(false);
    playerSpawnLocation = Player.transform.position;
    displayPlayerHealth();
  }

  public void add_score(int amount)
  {
    score += amount;
    if (score == rampScore && SceneManager.GetActiveScene().name == "Level 2")
    {
      rampText.SetActive(true);
      ramp1.SetActive(true);
      ramp2.SetActive(true);
      beatLevelScore = 80;
    }
    if (score >= beatLevelScore)
    {
      scoreText.text = "Score = " + score.ToString() + " of " + beatLevelScore.ToString();
      if (score >= beatLevelScore)
      {
        winText.SetActive(true);
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
          NextLevelButton.gameObject.SetActive(true);
        }
        else
        {
          ResetGameButton.gameObject.SetActive(true);
        }
        PlayAgainButton.gameObject.SetActive(true);
        audioSource.Stop();
        playAudioOneTime(beatLevelSFX);
        destroyAllEnemy();
        pauseGame = true;
        pause();
      }
    }
    else scoreText.text = "Score = " + score.ToString();
  }

  public void pause()
  {
    float enemySpeed;
    if (pauseGame) enemySpeed = 0;
    else enemySpeed = originalSpeed;
    GameObject[] enemy = GameObject.FindGameObjectsWithTag("EnemyTag");
    if (enemy.Length > 0)
    {
      originalSpeed = enemy[0].GetComponent<Chase>().speed; // Save only once
    }
    foreach (GameObject g in enemy)
    {
      g.GetComponent<Chase>().speed = enemySpeed;
    }
  }

  public void decHealth()
  {
    playerHealth -= 1;
    displayPlayerHealth();
    if (playerHealth == 0)
    {
      GameOverText.SetActive(true);
      PlayAgainButton.gameObject.SetActive(true);
      // gameState = gameStates.GameOver; 
      audioSource.Stop();
      pauseGame = true;
      playAudioOneTime(gameOverSFX);
    }
    else destroyAllEnemy();
  }

  void destroyAllEnemy()
  {
    GameObject[] enemy = GameObject.FindGameObjectsWithTag("EnemyTag");
    foreach (GameObject g in enemy)
    {
      Destroy(g);
    }
  }


  void displayPlayerHealth()
  {
    healthText.text = "Health: " + playerHealth.ToString();
  }


  void SpawnCoin()
  {
    //if (pauseGame) return;
    float randomX = UnityEngine.Random.Range(-maxX, maxX);
    float randomZ = UnityEngine.Random.Range(-maxZ, maxZ);
    Vector3 randomSpawnPos = new Vector3(randomX, 10f, randomZ);
    Instantiate(CoinPrefab, randomSpawnPos, Quaternion.identity);  // create a "ball" body at a random position, facing identity.
  }
  void SpawnEnemy()
  {
    if (pauseGame) return;
    int randomCorner = UnityEngine.Random.Range(0, 4);
    Instantiate(EnemyPrefab, spawnPoint[randomCorner].transform.position, Quaternion.identity);  // create a "ball" body at a random position, facing identity.
  }

  public void playAgain()
  {
    setupDefaults();
    audioSource.Stop();
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);

  }
  public void nextLevel()
  {
    setupDefaults();
    audioSource.Stop();
    SceneManager.LoadScene("Level 2");
  }

  public void resetGame()
  {
    setupDefaults();
    audioSource.Stop();
    SceneManager.LoadScene("Level 1");

  }
  void playAudioRepeat(AudioClip clip)
  {
    audioSource = cam.GetComponent<AudioSource>();
    audioSource.clip = clip;
    audioSource.Play();
  }
  void playAudioOneTime(AudioClip clip)
  {
    AudioSource.PlayClipAtPoint(clip, cam.transform.position);
  }




}







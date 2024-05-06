using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float gameTime = 0f;
    public bool isGameStarted = false;

    private GameObject player;
    private PlayerController playerController;
    private HealthController playerHealth;
    private GameObject spawnPoint;
    private FinishController finishController;
    public VolumeExpoLerper volumeExpoLerper;
    public CameraController cameraController;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // Create GameManager if not present in the scene
        if (FindObjectOfType<GameManager>() == null)
        {
            GameObject managerObj = new GameObject("GameManager");
            managerObj.AddComponent<GameManager>();
        }
    }

    private void Start()
    {
        InitializeGame();
    }

    private void Update()
    {
        if (isGameStarted)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartLevel();
                return;
            }
            gameTime += Time.deltaTime;
            if (gameTime >= 10)
            {
                EndGame();
            }
        }
    }

    public void InitializeGame()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<HealthController>();
        playerHealth.OnDie += () =>
        {
            isGameStarted = false;
            Invoke("EndGame", 1f);
        };
        playerController = player.GetComponent<PlayerController>();
        playerController.FreezePlayer(true);

        spawnPoint = GameObject.FindGameObjectWithTag("Spawnpoint");
        finishController = GameObject.FindWithTag("Finish").GetComponent<FinishController>();
        finishController.onFinish += LevelCompleted;
        cameraController = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();

        volumeExpoLerper = GameObject.FindWithTag("GlobalVolume").GetComponent<VolumeExpoLerper>();

        ResetGame();
        volumeExpoLerper.ChangeFromTo(-10f, 0f, 1f, () =>
        {
            StartGame();
        });

        Debug.Log("Game...");
        Debug.Log(volumeExpoLerper);
    }

    public void StartGame()
    {
        isGameStarted = true;
        playerController.FreezePlayer(false);
        Debug.Log("started");
    }

    public void EndGame()
    {
        isGameStarted = false;
        playerController.FreezePlayer(true);
        ResetGame();
        volumeExpoLerper.ChangeFromTo(10f, 0f, 1f, () =>
        {
            StartGame();
        });
    }

    public void ResetGame()
    {
        isGameStarted = false;
        gameTime = 0f;
        player.transform.position = spawnPoint.transform.position;
        playerHealth.Reset();
        playerController.FreezePlayer(true);
    }

    public void LevelCompleted()
    {
        isGameStarted = false;
        Debug.Log("level completed");
        playerController.FreezePlayer(true);

        volumeExpoLerper.ChangeFromTo(0f, -10f, 1f, () =>
        {
            //change scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}

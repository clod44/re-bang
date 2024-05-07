using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float gameTime = 0f;
    public bool isGameStarted = false;

    public GameObject player;
    public PlayerController playerController;
    public HealthController playerHealth;
    private GameObject spawnPoint;
    private FinishController finishController;
    public VolumeExpoLerper volumeExpoLerper;
    public CameraController cameraController;
    public SoundEffectData[] sounds;

    [Serializable]
    public class SoundEffectData
    {
        public string name;
        public AudioSource audioSource;
    }


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

        if (FindObjectOfType<GameManager>() == null)
        {
            GameObject managerObj = new GameObject("GameManager");
            managerObj.AddComponent<GameManager>();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeGame();
    }

    private void Start()
    {
        InitializeGame();
    }
    private float beepT = 0f;
    private void Update()
    {
        if (isGameStarted)
        {
            beepT += Time.deltaTime;
            if (beepT >= 1f)
            {
                PlaySound("beep");
                beepT = 0f;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                EndGame();
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
        PlaySound("ambience");
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<HealthController>();
            playerHealth.OnDie += () =>
            {
                isGameStarted = false;
                if (player != null)
                    playerController.FreezePlayer(true);
                Invoke("EndGame", 1f);
            };
            playerController = player.GetComponent<PlayerController>();
            playerController.FreezePlayer(true);
        }
        spawnPoint = GameObject.FindGameObjectWithTag("Spawnpoint");
        GameObject finish = GameObject.FindWithTag("Finish");
        if (finish != null)
            finishController = finish.GetComponent<FinishController>();
        if (finishController != null)
            finishController.onFinish += LevelCompleted;
        GameObject camera = GameObject.FindWithTag("MainCamera");
        if (camera != null)
            cameraController = camera.GetComponent<CameraController>();
        GameObject volume = GameObject.FindWithTag("GlobalVolume");
        if (volume != null)
            volumeExpoLerper = volume.GetComponent<VolumeExpoLerper>();

        if (player != null && spawnPoint != null)
            ResetGame();

        volumeExpoLerper.ChangeFromTo(-10f, 0f, 1f, () =>
        {
            if (player != null && spawnPoint != null)
                StartGame();
        });

        Debug.Log("Game...");
        Debug.Log(volumeExpoLerper);
    }

    public void StartGame()
    {
        isGameStarted = true;
        if (player != null)
            playerController.FreezePlayer(false);
        Debug.Log("started");
    }

    public void EndGame()
    {
        PlaySound("flashbang");
        isGameStarted = false;
        if (player != null)
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

        if (player != null && spawnPoint != null)
        {
            player.transform.position = spawnPoint.transform.position;
            playerHealth.Reset();
            playerController.FreezePlayer(true);
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerHealth = player.GetComponent<HealthController>();
            playerController = player.GetComponent<PlayerController>();
            spawnPoint = GameObject.FindGameObjectWithTag("Spawnpoint");

            if (player != null && spawnPoint != null)
            {
                player.transform.position = spawnPoint.transform.position;
                playerHealth.Reset();
                playerController.FreezePlayer(true);
            }
            else
            {
                Debug.LogWarning("Player or spawn point not found.");
            }
        }
    }

    public void PlaySound(string name)
    {
        foreach (SoundEffectData sound in sounds)
        {
            if (sound.name == name)
            {
                sound.audioSource.PlayOneShot(sound.audioSource.clip);
                break;
            }
        }
    }

    public void LevelCompleted()
    {
        isGameStarted = false;
        Debug.Log("level completed");

        if (player != null)
            playerController.FreezePlayer(true);

        volumeExpoLerper.ChangeFromTo(0f, -10f, 1f, () =>
        {
            //change scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}

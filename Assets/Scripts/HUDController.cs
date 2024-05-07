using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI clockText;
    private Button pauseButton;
    [SerializeField]
    private Sprite[] pauseButtonTextures;
    [SerializeField]
    private Slider playerHealthSlider;
    private float desiredPlayerHealthSliderValue = 0f;
    void Start()
    {
        enabled = true;
        desiredPlayerHealthSliderValue = playerHealthSlider.value;
        GameManager.instance.playerHealth.OnHealthChange += UpdatePlayerHealth;
    }

    void Update()
    {
        float gameTime = GameManager.instance.gameTime;
        clockText.text = FormateGameTime(gameTime);
        playerHealthSlider.value = Mathf.Lerp(playerHealthSlider.value, desiredPlayerHealthSliderValue, 4f * Time.deltaTime);

    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PauseGame()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        pauseButton.GetComponent<Image>().sprite = pauseButtonTextures[(int)Time.timeScale];
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RewindLevel()
    {
        GameManager.instance.EndGame();
    }

    public void UpdatePlayerHealth(float newHealth, float maxHealth)
    {
        playerHealthSlider.maxValue = maxHealth;
        desiredPlayerHealthSliderValue = newHealth;
    }
    string FormateGameTime(float seconds)
    {
        seconds = 10f - seconds;
        int sec = (int)seconds;
        int ms = (int)((seconds - sec) * 100);

        return string.Format("{0:00}:{1:00}", sec, ms);
    }



}

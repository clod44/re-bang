using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {

    }

    public void StartLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}

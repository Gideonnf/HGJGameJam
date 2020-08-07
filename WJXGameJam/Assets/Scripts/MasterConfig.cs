using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterConfig : MonoBehaviour
{
    public bool master_isEndless;
    public FoodStage master_foodStage = FoodStage.Chinatown;
    public GameObject livesBox;
    public TextMeshProUGUI livesText;
    public int master_currentDay = 0;

    private bool pauseToggle = false;

    private static MasterConfig checkInstance;
    // Start is called before the first frame update
    void Start()
    {
        if (checkInstance == null)
        {
            checkInstance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (master_isEndless)
            livesBox.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (master_isEndless && SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MenuStage"))
        {
            livesText.text = "Lives: " + playerData.Lives;
        }
    }

    public void ReturnButton()
    {
        if (Time.timeScale != 1)
            Time.timeScale = 1;

        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            GameObject.Find("Background").GetComponent<MainMenuController>().BackToTitleScreen();
        }
        else
        {
            SceneManager.LoadSceneAsync("MenuScene", LoadSceneMode.Single);
        }
    }

    public void Pause()
    {
        pauseToggle = !pauseToggle;

        if (pauseToggle)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}

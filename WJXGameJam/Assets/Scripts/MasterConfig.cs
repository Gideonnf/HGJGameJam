using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterConfig : MonoBehaviour
{
    public bool master_isEndless;
    public FoodStage master_foodStage = FoodStage.Chinatown;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnButton()
    {
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

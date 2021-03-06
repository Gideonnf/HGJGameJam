﻿using System.Collections;
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
    private string ButtonClick = "ButtonClick";
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
        if (master_isEndless && SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MenuScene"))
        {
            livesText.text = "Lives: " + playerData.Lives;
        }
    }

    public void ReturnButton()
    {
        if (Time.timeScale != 1)
            Time.timeScale = 1;

        SoundManager.Instance.Play(ButtonClick);

        if (SceneManager.GetActiveScene().name == "MenuScene")
        {
            playerData.ResetPlayerLives();

            GameObject.Find("Background").GetComponent<MainMenuController>().BackToTitleScreen();
        }
        else
        {
            SceneManager.LoadSceneAsync("MenuScene", LoadSceneMode.Single);
        }
    }

    public void Pause()
    {
        SoundManager.Instance.Play(ButtonClick);
        pauseToggle = !pauseToggle;

        if (pauseToggle)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}

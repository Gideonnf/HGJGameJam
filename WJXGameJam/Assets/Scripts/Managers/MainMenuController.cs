using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Serializable]
    private struct Panels
    {
        public string name;
        public GameObject panel;
    }
    [SerializeField]
    private Panels[] panelHolder;

    public Dictionary<string, GameObject> PanelsList = new Dictionary<string, GameObject>();

    private string currentActivePanel = "MainMenu";
    private bool showingHelp = false;
    private string ButtonClick = "ButtonClick";

    private MasterConfig MC;
    // Start is called before the first frame update
    void Start()
    {
        MC = GameObject.Find("DoNotDestroy").GetComponent<MasterConfig>();

        for (int i = 0; i < panelHolder.Length; ++i)
        {
            PanelsList.Add(panelHolder[i].name, panelHolder[i].panel);
            panelHolder[i].panel.SetActive(false);
        }

        PanelsList[currentActivePanel].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackToTitleScreen()
    {
        for (int i = 0; i < panelHolder.Length; ++i)
        {
            panelHolder[i].panel.SetActive(false);
        }
        SoundManager.Instance.Play(ButtonClick);
        PanelsList["MainMenu"].SetActive(true);
    }

    public void toggleHelp()
    {
        showingHelp = !showingHelp;
        SoundManager.Instance.Play(ButtonClick);
        PanelsList["Help panel"].SetActive(showingHelp);
    }

    public void SetGameMode(bool isEndless)
    {
        MC.master_isEndless = isEndless;
        MC.livesBox.SetActive(isEndless);
        SoundManager.Instance.Play(ButtonClick);
        ChangePanel("Career picker");
    }

    public void ChangePanel(string panelName)
    {
        PanelsList[currentActivePanel].SetActive(false);
        PanelsList[panelName].SetActive(true);
    }

    public void SetEndlessMode(int stage)
    {
        MC.master_foodStage = (FoodStage)stage;
        SceneManager.LoadSceneAsync((int)MC.master_foodStage + 1, LoadSceneMode.Single);
    }

    public void SetCareerProgress(int stage)
    {
        MC.master_foodStage = (FoodStage)stage;

        switch ((int)MC.master_foodStage)
        {
            case 0:
                {
                    MC.master_currentDay = 0;

                    break;
                }
            case 1:
                {
                    MC.master_currentDay = 5;

                    break;
                }
            case 2:
                {
                    MC.master_currentDay = 10;

                    break;
                }
            case 3:
                {
                    MC.master_currentDay = 15;

                    break;
                }
            default:
                break;
        }

        SceneManager.LoadSceneAsync((int)MC.master_foodStage + 1, LoadSceneMode.Single);
    }
}

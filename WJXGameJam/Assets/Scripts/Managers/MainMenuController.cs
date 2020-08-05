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

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < panelHolder.Length; ++i)
        {
            PanelsList.Add(panelHolder[i].name, panelHolder[i].panel);
        }

        PanelsList[currentActivePanel].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGameMode(bool isEndless)
    {
        MasterConfig.Instance.master_isEndless = isEndless;

        if (!MasterConfig.Instance.master_isEndless)
            ChangePanel("Career picker");
        else
            SceneManager.LoadSceneAsync("ChinaTownScene", LoadSceneMode.Single);
    }

    public void ChangePanel(string panelName)
    {
        PanelsList[currentActivePanel].SetActive(false);
        PanelsList[panelName].SetActive(true);
    }
    public void SetCareerProgress(int stage)
    {
        MasterConfig.Instance.master_foodStage = (FoodStage)stage;

        switch ((int)MasterConfig.Instance.master_foodStage)
        {
            case 0:
                {
                    MasterConfig.Instance.master_currentDay = 0;

                    break;
                }
            case 1:
                {
                    MasterConfig.Instance.master_currentDay = 5;

                    break;
                }
            case 2:
                {
                    MasterConfig.Instance.master_currentDay = 10;

                    break;
                }
            case 3:
                {
                    MasterConfig.Instance.master_currentDay = 15;

                    break;
                }
            default:
                break;
        }

        SceneManager.LoadSceneAsync((int)MasterConfig.Instance.master_foodStage + 1, LoadSceneMode.Single);
    }
}

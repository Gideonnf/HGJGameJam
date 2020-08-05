using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum FoodType
{
    Plate,
    Cup,
    Poop
}
public enum FoodStage
{
    Chinatown,
    GeylangSerai,
    LittleIndia,
    Coffeeshop
}
public enum MainIngredient
{
    NoIngredient,
    // China Town Main Ingredients
    Rice,
    Noodle,
    // Geylang Serai Main Ingredients
    GrillChicken,
    GrillMutton,
    GrillBeef,
}

public enum SubIngredient
{
    // Shared Sub Ingredients,
    Egg,
    Onion,
    // China Town Sub Ingredients
    RoastChicken,
    RoastDuck,
    RoastPork,
    Wanton,
    // Geylang Serai Sub Ingredients
    PeanutSauce,
    Ketapult,
    Otah
}

public class DataManager : SingletonBase<DataManager>
{
    public Canvas ref_canvas;

    [Tooltip("Round time in seconds")]
    public int roundTime = 10;

    //1 - endless gamemode, 0 - career mode
    public bool isEndless = true;
    public int currentDay { get; set; }
    public bool roundStart { get; set; }

    public int maxDays = 20;
    private bool waitLastCustomer = false;
    Stopwatch timer = new Stopwatch();

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();

        if (GameObject.Find("DoNotDestroy") != null)
        {
            currentDay = MasterConfig.Instance.master_currentDay;
            isEndless = MasterConfig.Instance.master_isEndless;
            CustomerManager.Instance.m_CurrFoodStage = MasterConfig.Instance.master_foodStage;
        }
        else
        {
            currentDay = 0;

            isEndless = true;
            roundStart = false;
        }

        if (isEndless)
        {
            playerData.moneyPerDay.Add(0);
            playerData.dishesPerDay.Add(0);
            playerData.customersPerDay.Add(0);
        }
    }

    public void Start()
    {
        if (GameObject.Find("DoNotDestroy") != null)
            TransitionManager.Instance.startTransition = true;
    }
    public void StartDay()
    {
        //TODO: Start day functions
        ++currentDay;

        CustomerManager.Instance.m_CurrDifficulty = currentDay / maxDays;

        //Add new value for next day
        playerData.moneyPerDay.Add(0);
        playerData.dishesPerDay.Add(0);

        roundStart = true;
        timer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (roundStart)
        {
            if (timer.Elapsed.TotalSeconds >= (double)roundTime)
            {
                timer.Stop();
                timer.Reset();

                roundStart = false;
                //TODO: End of day functions

                if (CustomerManager.Instance.m_CurrentCustomersInQueue == 0)
                {
                    //TODO: NEXT STAGE
                    //ease out
                    TransitionManager.Instance.easeIn = false;
                    TransitionManager.Instance.startTransition = true;

                    FoodManager.Instance.DayOfJudgement();

                    if (currentDay % 5 == 0)
                    {
                        ++MasterConfig.Instance.master_foodStage;
                        SceneManager.LoadSceneAsync((int)MasterConfig.Instance.master_foodStage + 1);
                    }
                    else
                        ++currentDay;

                    MasterConfig.Instance.master_currentDay = currentDay;
                }
                else
                {
                    waitLastCustomer = true;
                }
            }
        }

        if (waitLastCustomer)
        {
            if (CustomerManager.Instance.m_CurrentCustomersInQueue == 0)
            {
                waitLastCustomer = false;
                //TODO: NEXT STAGE
                //same as 92
                //ease out
                TransitionManager.Instance.easeIn = false;
                TransitionManager.Instance.startTransition = true;

                FoodManager.Instance.DayOfJudgement();

                if (currentDay % 5 == 0)
                {
                    ++MasterConfig.Instance.master_foodStage;
                    SceneManager.LoadSceneAsync((int)MasterConfig.Instance.master_foodStage + 1);
                }
                else
                    ++currentDay;

                MasterConfig.Instance.master_currentDay = currentDay;
            }
        }

    }
}

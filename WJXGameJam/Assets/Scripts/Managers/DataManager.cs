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
    // Little India ingredients
    CurrySauce,
    ThreeSauces,
}

public enum SubIngredient
{
    // Shared Sub Ingredients,
    Onion,
    // China Town Sub Ingredients
    RoastChicken,
    RoastDuck,
    RoastPork,
    Wanton,
    // Geylang Serai Sub Ingredients
    PeanutSauce,
    Ketapult,
    Otah,
    // Little India Sub Ingredients
    CheesePrata,
    EggPrata,
    PlainPrata,
    OnionPrata,
    Thosai,

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

    private MasterConfig MC;
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();

        if (GameObject.Find("DoNotDestroy") != null)
        {
            MC = GameObject.Find("DoNotDestroy").GetComponent<MasterConfig>();
            currentDay = MC.master_currentDay;
            isEndless = MC.master_isEndless;
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

    private void Start()
    {
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
                        if (MC.master_foodStage == FoodStage.LittleIndia)
                        {
                            MC.master_currentDay = 0;
                        }
                        else
                        {
                            ++MC.master_foodStage;
                        }
                    }

                    MC.master_currentDay = currentDay;
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
                    if (MC.master_foodStage == FoodStage.LittleIndia)
                    {
                        MC.master_currentDay = 0;
                    }
                    else
                    {
                        ++MC.master_foodStage;
                    }
                }

                MC.master_currentDay = currentDay;
            }
        }

        if (isEndless)
        {
            if (playerData.Lives == 0)
            {
                TransitionManager.Instance.easeIn = false;
                TransitionManager.Instance.startTransition = true;
            }
        }
    }

    public void NextButton()
    {
        if (isEndless)
        {
            SceneManager.LoadSceneAsync("MenuScene");
        }
        else
        {
            if (currentDay % 5 == 0)
            {
                if (currentDay == 15 && MC.master_foodStage == FoodStage.LittleIndia)
                {
                    SceneManager.LoadSceneAsync("MenuScene");
                }
                else
                {
                    SceneManager.LoadSceneAsync((int)MC.master_foodStage + 1);
                }

                return;
            }

            TransitionManager.Instance.startTransition = true;
        }
    }
}

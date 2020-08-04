using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

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
    Ketapult
}

public class DataManager : SingletonBase<DataManager>
{
    public Canvas ref_canvas;

    public int currentDay { get; set; }
    public bool roundStart { get; set; }

    Stopwatch timer = new Stopwatch();

    // Start is called before the first frame update
    void Start()
    {
        currentDay = 0;
        roundStart = false;
    }

    public void StartDay()
    {
        //TODO: Start day functions
        ++currentDay;

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
            if (timer.Elapsed.TotalSeconds >= (double)90) //close end panel after 4 secs
            {
                timer.Stop();
                timer.Reset();

                roundStart = false;
                //TODO: End of day functions


            }
        }
    }
}

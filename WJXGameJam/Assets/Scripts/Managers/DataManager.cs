using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodStage
{
    Chinatown,
    GeylangSerai,
    LittleIndia,
    Coffeeshop
}
public enum MainIngredient
{
    // China Town Main Ingredients
    Rice,
    Noodle,
    Wanton,
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
    CharSiew,
    // Geylang Serai Sub Ingredients
    PeanutSauce,
    Ketapult
}

public class DataManager : SingletonBase<DataManager>
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

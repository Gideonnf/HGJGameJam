﻿using System.Collections;
using System.Collections.Generic;
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
    CharSiew,
    Wanton,
    // Geylang Serai Sub Ingredients
    PeanutSauce,
    Ketapult
}

public class DataManager : SingletonBase<DataManager>
{
    public Canvas ref_canvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

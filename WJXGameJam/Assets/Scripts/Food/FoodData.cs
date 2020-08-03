using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FoodData
{
    [Tooltip("The type of food the dish is. What is it gonna be the base. A cup or plate? Poop maybe")]
    public FoodType foodType = FoodType.Poop;

    [Tooltip("The main ingredient of the dish. Each dish should only have one")]
    public MainIngredient mainIngredient = MainIngredient.NoIngredient;

    [Tooltip("List of the food sub ingredients. Can have one or more")]
    public List<SubIngredient> ListOfSubIngredients = new List<SubIngredient>();

    [Tooltip("What stage the food is in")]
    public FoodStage foodStage;

    [Tooltip("Cost of Food")]
    public float totalCost;

    [NonSerialized]
    [Tooltip("Food Tag used to pull from object pooler")]
    public string foodTag;
}

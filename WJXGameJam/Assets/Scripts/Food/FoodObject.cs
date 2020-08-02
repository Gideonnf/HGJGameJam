using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodObject", menuName = "ScriptableObjects/FoodObject")]
public class FoodObject : ScriptableObject
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

    [Tooltip("Sprite of the object")]
    public Sprite foodSprite;

    /// <summary>
    /// Adding sub ingredients to the list
    /// Used when creating dishes in the kitchen
    /// Adds up the price as well
    /// </summary>
    /// <param name="IngredientToAdd"> Pass in the ingredient Object into the function </param>
    public bool AddIngredient(IngredientObject IngredientToAdd)
    {
        // It cant add cause they belong to the wrong food types
        // u cant add half boil eggs to a cup idiot
        if (foodType != IngredientToAdd.foodType)
            return false;

        // If it is a main ingredient
        if (IngredientToAdd.IsMain)
        {
            // Check if it has no main ingredient yet
            if (mainIngredient == MainIngredient.NoIngredient)
                mainIngredient = IngredientToAdd.mainIngredient;
            else
                return false;
        }   
        // It is a sub ingredient
        else
        {
            // Add it to the sub ingredient list
            ListOfSubIngredients.Add(IngredientToAdd.subIngredient);
        }

        // Add up the cost
        totalCost += IngredientToAdd.ingredientCost;

        return true;
    }

}

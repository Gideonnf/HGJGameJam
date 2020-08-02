using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodObject", menuName = "ScriptableObjects/FoodObject")]
public class FoodObject : ScriptableObject
{
    [Tooltip("The main ingredient of the dish. Each dish should only have one")]
    public MainIngredient mainIngredient;

    [Tooltip("List of the food sub ingredients. Can have one or more")]
    public List<SubIngredient> ListOfSubIngredients = new List<SubIngredient>();

    [Tooltip("What stage the food is in")]
    public FoodStage foodStage;

    [Tooltip("Cost of Food")]
    public float totalCost;

    /// <summary>
    /// Adding sub ingredients to the list
    /// Used when creating dishes in the kitchen
    /// Adds up the price as well
    /// </summary>
    /// <param name="IngredientToAdd"> Pass in the ingredient Object into the function </param>
    public void AddSubIngredient(IngredientObject IngredientToAdd)
    {
        ListOfSubIngredients.Add(IngredientToAdd.subIngredient);

        totalCost += IngredientToAdd.ingredientCost;
    }

}

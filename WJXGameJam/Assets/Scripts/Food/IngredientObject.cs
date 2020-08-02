using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientObject : MonoBehaviour
{
    [Tooltip("Is it a main ingredient")]
    public bool MainIngredient;

    [Tooltip("Cost of Ingredient")]
    public float ingredientCost;

    [Tooltip("The Sub Ingredient of the dish if it isnt a main")]
    public SubIngredient subIngredient;

    [Tooltip("The Main Ingredient of the dish if its a main")]
    public MainIngredient mainIngredient;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

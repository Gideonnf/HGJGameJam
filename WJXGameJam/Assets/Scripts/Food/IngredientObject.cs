using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

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

    [Tooltip("Time Taken to Prepare before the ingredient is ready to eat/combine")]
    public float timeToPrepare;

    [System.NonSerialized]
    [Tooltip("Is it cooking/being prepared")]
    public bool isPreparing;

    // Keep track of time for cooking
    float timeElapsed;    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPreparing)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= timeToPrepare)
            {
                // The food is done preparing
                // Can set the animation state to finished or smth like that?
            }
        }
    }
}

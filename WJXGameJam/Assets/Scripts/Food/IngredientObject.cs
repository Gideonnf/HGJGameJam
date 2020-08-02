using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
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
    [Tooltip("WhAt Is ThE cHeF cOoKiNg!!!")]
    public bool isPreparing = false;

    [System.NonSerialized]
    [Tooltip("pee pee poo poo")]
    public bool isDone = false;

    // Keep track of time for cooking
    float timeElapsed;    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Testing purposes
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPreparing = true;

            // If its done already
            if (isDone == true)
            {

            }
        }

        if (isPreparing)
        {
            // poop finish haha
            if (isDone)
                return;

            timeElapsed += Time.deltaTime;
            if (timeElapsed >= timeToPrepare)
            {
                isDone = true;
                // The food is done preparing
                // Can set the animation state to finished or smth like that?
            }
        }
    }
}

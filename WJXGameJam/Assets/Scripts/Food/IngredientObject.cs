using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Timeline;

public class IngredientObject : MonoBehaviour
{
    [Tooltip("Is it a main ingredient")]
    public bool IsMain;

    [Tooltip("Cost of Ingredient")]
    public float ingredientCost;

    [Tooltip("What is the food gonna be on. Plate, Cup or poop")]
    public FoodType foodType = FoodType.Poop;

    [Tooltip("The Main Ingredient of the dish if its a main")]
    public MainIngredient mainIngredient;

    [Tooltip("The Sub Ingredient of the dish if it isnt a main")]
    public SubIngredient subIngredient;

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
                IngredientObject ingredient = gameObject.GetComponent<IngredientObject>();

                if(FoodManager.Instance.TestingFunction(ingredient))
                {
                    Destroy(this.gameObject);
                }
            }
        }

        if (isPreparing)
        {
            // poop finish haha
            if (isDone)
                return;

            timeElapsed += Time.deltaTime;

            // poopy
            Debug.Log("Time spent cooking" + timeElapsed);

            if (timeElapsed >= timeToPrepare)
            {
                isDone = true;
                // The food is done preparing
                // Can set the animation state to finished or smth like that?
            }
        }
    }
}

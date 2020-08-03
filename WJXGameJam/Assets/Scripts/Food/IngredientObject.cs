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

    DraggableObjectController DraggableReference;

    // Keep track of time for cooking
    float timeElapsed;    

    // Start is called before the first frame update
    void Start()
    {
        DraggableReference = GetComponent<DraggableObjectController>();
    }

    public void OnMouseUp()
    {
        // check if colliding with a dish

        // add to dish

    }

    // Update is called once per frame
    void Update()
    {
        // Testing purposes
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    isPreparing = true;

        //    // If its done already
        //    if (isDone == true)
        //    {
        //        IngredientObject ingredient = gameObject.GetComponent<IngredientObject>();

        //        //if(FoodManager.Instance.TestingFunction(ingredient))
        //        //{
        //        //    Destroy(this.gameObject);
        //        //}
        //    }
        //}

        CheckForIngredientDrop();

        // check if still pooping
        if (isDone == false)
            CheckForCooking();

    }

    private void OnEnable()
    {
        isPreparing = true;
    }

    bool CheckForIngredientDrop()
    {
        // If it isnt done yet then return false
        if (!isDone)
        {
            DraggableReference.ResetPosition();

            return false;
        }


        // If it is colliding with a drop location
        if (DraggableReference.collisionInfo != null)
        {
            // If the mouse is up i.e they have stopped dragging
            if (DraggableReference.GetDragging() == false)
            {
                IngredientObject ingredient = gameObject.GetComponent<IngredientObject>();

                if(DraggableReference.collisionInfo.gameObject.GetComponent<FoodObject>().AddIngredient(ingredient))
                {
                    Debug.Log("Ingredient successfully added");

                    // Set up the sprite after adding
                    DraggableReference.collisionInfo.gameObject.GetComponent<FoodObject>().SetUpSprite();

                    Destroy(gameObject);

                    //TODO:: Clear the ingredient from the foodmanager prepslots
                }
                else
                {
                    Debug.Log("Ingredient failed to add");

                    DraggableReference.ResetPosition();

                }
            }
        }


        return true;
    }

    bool CheckForCooking()
    {
        if (isPreparing)
        {
            timeElapsed += Time.deltaTime;

            // poopy
            Debug.Log("Time spent cooking" + timeElapsed);

            if (timeElapsed >= timeToPrepare)
            {
                isDone = true;
                // The food is done preparing
                // Can set the animation state to finished or smth like that?
                return true;
            }
        }

        return false;

    }
}

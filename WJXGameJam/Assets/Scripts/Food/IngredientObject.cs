using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Timeline;

public class IngredientObject : MonoBehaviour
{
    [Tooltip("Cost of Ingredient")]
    public float ingredientCost;

    [Tooltip("What is the food gonna be on. Plate, Cup or poop")]
    public FoodType foodType = FoodType.Poop;

    [Tooltip("Time Taken to Prepare before the ingredient is ready to eat/combine")]
    public float timeToPrepare;

    [Header("Define the ingredient")]
    [Tooltip("Is it a main ingredient")]
    public bool IsMain;

    [Tooltip("The Main Ingredient of the dish if its a main")]
    public MainIngredient mainIngredient;

    [Tooltip("The Sub Ingredient of the dish if it isnt a main")]
    public SubIngredient subIngredient;

    [HideInInspector]
    [Tooltip("Tag for the food prefab that was used to pull it from object pooler")]
    public string foodTag;

    [HideInInspector]
    [Tooltip("WhAt Is ThE cHeF cOoKiNg!!!")]
    public bool isPreparing = false;

    [HideInInspector]
    [Tooltip("pee pee poo poo")]
    public bool isDone = false;

    [HideInInspector]
    // Keep track of time for cooking
    public float timeElapsed;
    DraggableObjectController DraggableReference;

    // Start is called before the first frame update
    void Start()
    {
        DraggableReference = GetComponent<DraggableObjectController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForIngredientDrop();

        // check if still pooping
        if (isPreparing)
            CheckForCooking();

    }

    private void OnEnable()
    {
        isPreparing = true;
    }

    //reset object
    private void OnDisable()
    {
        isPreparing = false;
        isDone = false;
        this.transform.parent = null;
        timeElapsed = 0.0f;

        //resetting
        FoodManager.Instance.RemoveFromPrepSlot(gameObject);
    }

    bool CheckForIngredientDrop()
    {
        // If it isnt done yet then return false
        if (!isDone)
        {
            DraggableReference.ResetPosition();
            this.transform.parent = null;

            return false;
        }

        // if its parented means its collided with the dish
        if (transform.parent != null)
        {
            IngredientObject ingredient = gameObject.GetComponent<IngredientObject>();
            // check if it can be added
            if(GetComponentInParent<FoodObject>().AddIngredient(ingredient))
            {
                // Edit the sprite to fit stuff
                GetComponentInParent<FoodObject>().SetUpSprite();

                FoodManager.Instance.RemoveFromPrepSlot(gameObject);

                // unparent it
                gameObject.transform.parent = null;

                // set back to inactive for the object pooler
                gameObject.SetActive(false);
            }
            else
            {
                DraggableReference.ResetPosition();

                gameObject.transform.parent = null;


                return false;
            }
        }

        //TODO:: Dont allow for wanton and pork at the same time


        // If it is colliding with a drop location
        //if (DraggableReference.collisionInfo != null)
        //{
        //    // If the mouse is up i.e they have stopped dragging
        //    if (DraggableReference.GetDragging() == false)
        //    {
        //        IngredientObject ingredient = gameObject.GetComponent<IngredientObject>();

        //        if(DraggableReference.collisionInfo.gameObject.GetComponent<FoodObject>().AddIngredient(ingredient))
        //        {
        //            Debug.Log("Ingredient successfully added");

        //            // Set up the sprite after adding
        //            DraggableReference.collisionInfo.gameObject.GetComponent<FoodObject>().SetUpSprite();

        //            FoodManager.Instance.RemoveFromPrepSlot(gameObject);

        //            Destroy(gameObject);
        //        }
        //        else
        //        {
        //            Debug.Log("Ingredient failed to add");

        //            DraggableReference.ResetPosition();

        //        }
        //    }
        //}


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
                // The food is done preparing
                if (this.gameObject.GetComponent<FoodSpriteChanger>() == null)
                {
                    isDone = true;
                    isPreparing = false;
                }

                return true;
            }
        }

        return false;

    }
}

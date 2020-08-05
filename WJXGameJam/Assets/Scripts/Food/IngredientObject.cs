using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Timeline;

public class IngredientObject : MonoBehaviour
{
    // for objects that can only be dragged
    [Tooltip("Object will be deactivated if it isnt being dragged")]
    public bool OnlyDraggable = false;

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
       // Debug.Log(gameObject.name + " dragging is " + DraggableReference.isDragging);

        CheckForIngredientDrop();

        if (OnlyDraggable)
        {
            // If it isnt being dragged
            if (DraggableReference.GetDragging() == false)
            {
                // And it isnt within a collider
                if (DraggableReference.inCollider == false)
                {
                    gameObject.SetActive(false);
                }
            }
        }

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
        if (!FoodManager.m_ShuttingDown)
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

                if (!FoodManager.m_ShuttingDown)
                    FoodManager.Instance.RemoveFromPrepSlot(gameObject);

                // unparent it
                gameObject.transform.parent = null;

                // set back to inactive for the object pooler
                gameObject.SetActive(false);
            }
            else
            {
                if (OnlyDraggable)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    DraggableReference.ResetPosition();

                    gameObject.transform.parent = null;
                }
                return false;
            }
        }

        return true;
    }

    bool CheckForCooking()
    {
        if (isPreparing && !DraggableReference.isDragging)
        {
            timeElapsed += Time.deltaTime;

            // poopy
            // Debug.Log("Time spent cooking" + timeElapsed);

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

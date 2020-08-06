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

    [Tooltip("LITERALLY ONLY FOR FUCKING PRATA")]
    public bool ForPrata = false;

    bool HasThePrataBeenModifiedYetLol = false;

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

    private SubIngredient startingSubIngredient;

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
    void Awake()
    {
        DraggableReference = GetComponent<DraggableObjectController>();

        if (gameObject.GetComponent<PrataIndicator>())
        {
            startingSubIngredient = subIngredient;
        }
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(gameObject.name + " dragging is " + DraggableReference.isDragging);

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
        if (OnlyDraggable)
            return;

        isPreparing = false;
        isDone = false;
        this.transform.parent = null;
        timeElapsed = 0.0f;

        // if its a prata
        if (gameObject.GetComponent<PrataIndicator>())
        {
            if (gameObject.GetComponent<PrataIndicator>().IndicatorReference)
                gameObject.GetComponent<PrataIndicator>().IndicatorReference.SetActive(false);

            subIngredient = startingSubIngredient;

        }


        //resetting
        if (!FoodManager.m_ShuttingDown)
            FoodManager.Instance.RemoveFromPrepSlot(gameObject);
    }

    bool CheckForIngredientDrop()
    {
        if (this.gameObject.GetComponent<FoodSpriteChanger>() != null)
        {
            // If it isnt done yet then return false
            if (!isDone)
            {
                DraggableReference.ResetPosition();
                this.transform.parent = null;
                return false;
            }
            else
            {
                //types of doneness
                //some can be done and not overcooked (chicken)
                //some have a window to doneness
                if (this.gameObject.GetComponent<FoodSpriteChanger>().noOvercook && isPreparing) //if it can overcook, and its done, but is still preparing
                {
                    DraggableReference.ResetPosition();
                    this.transform.parent = null;
                    return false;
                }
                else if (!this.gameObject.GetComponent<FoodSpriteChanger>().noOvercook && !isPreparing) //if it cant overcook, and its done, but is not preparing
                {
                    DraggableReference.ResetPosition();
                    this.transform.parent = null;
                    return false;
                }
            }
        }

        // if its parented means its collided with the dish
        if (transform.parent != null)
        {
            // If its an ingredient object
            // this is only gonna be used for prata
            // cause that shit ghey
            if (transform.parent.GetComponent<IngredientObject>())
            {
                // get the prata ingredient object
                IngredientObject PrataIngredient = transform.parent.GetComponent<IngredientObject>();

                if (PrataIngredient.HasThePrataBeenModifiedYetLol == false)
                {
                    IngredientObject ingredient = gameObject.GetComponent<IngredientObject>();

                    // Set the sub ingredient of the prata to the one its being changed to
                    PrataIngredient.subIngredient = ingredient.subIngredient;

                    transform.parent.gameObject.GetComponent<PrataIndicator>().UpdateSubIngredient();

                    PrataIngredient.HasThePrataBeenModifiedYetLol = true;
                }


                // unparent it
                gameObject.transform.parent = null;

                // set back to inactive for the object pooler
                gameObject.SetActive(false);

            }
            else
            {
                // This is for everything else lol
                
                // JUS FOR THE FUKING PRATA
                // prata ingredients cant interact with a food object
                
                if (ForPrata)
                {
                    return false;
                }


                IngredientObject ingredient = gameObject.GetComponent<IngredientObject>();
                // check if it can be added
                if (GetComponentInParent<FoodObject>().AddIngredient(ingredient))
                {
                    // Edit the sprite to fit stuff
                    GetComponentInParent<FoodObject>().SetUpSprite();

                    if (!FoodManager.m_ShuttingDown)
                        FoodManager.Instance.RemoveFromPrepSlot(gameObject);

                    // unparent it
                    gameObject.transform.parent = null;

                    // if its a prata
                    if (gameObject.GetComponent<PrataIndicator>())
                    {
                        if (gameObject.GetComponent<PrataIndicator>().IndicatorReference)
                            gameObject.GetComponent<PrataIndicator>().IndicatorReference.SetActive(false);

                        subIngredient = startingSubIngredient;
                    }

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

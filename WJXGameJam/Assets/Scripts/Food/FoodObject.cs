using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class FoodObject : MonoBehaviour
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

    [System.NonSerialized]
    // Match the sub ingredeint to the integer value in ChildObjects
    // Example, RoastedChicken, 0
    // This dictionary is set up in a separate script attached to this object
    public Dictionary<SubIngredient, int> ChildSprites = new Dictionary<SubIngredient, int>();

    [Tooltip("Stores a list of all the children object")]
    // Keeps track of all the sub ingredients 
    // Allows for easy enabling and disabling of sub ingredients
    // Each sub ingredient is a new child object
    // The main object being the dish
    public List<GameObject> ChildObjects = new List<GameObject>();

    void Start()
    {
        //foodObject = new FoodObject();

        // Loop through the child objects
        foreach (Transform child in transform)
        {
            // Add them to the list
            ChildObjects.Add(child.gameObject);
            // Set active to false
            child.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // TESTING 
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // 
            if (AddSubIngredient(SubIngredient.CharSiew))
                Debug.Log("Successfully Added");
            else
                Debug.Log("Unsuccessful");

           // SetUpSprite();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (AddSubIngredient(SubIngredient.RoastChicken))
                Debug.Log("Successfully Added");
            else
                Debug.Log("Unsuccessful");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (AddSubIngredient(SubIngredient.RoastDuck))
                Debug.Log("Successfully Added");
            else
                Debug.Log("Unsuccessful");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetUpSprite();
        }
    }

    /// <summary>
    /// Call this function to add the ingredient object to the dish
    /// </summary>
    /// <param name="newIngredient">  </param>
    public bool AddToDish(IngredientObject newIngredient)
    {
        // Add it to the food object 
        if (AddIngredient(newIngredient))
        {
            // Toggle the sprite
            // Do the sprite thing later
            //ToggleSprites(newIngredient);

            return true;
        }

        return false;
    }


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

    #region For Randomisation
    // cuz need to manually add the ingredeitsn
    // laymao
    // poopiedy poo

    public bool AddMainIngredient(MainIngredient IngredientToAdd)
    {
        if (mainIngredient == MainIngredient.NoIngredient)
        {
            mainIngredient = IngredientToAdd;
            return true;  
        }

        return false;
    }

    public bool AddSubIngredient(SubIngredient IngredientToAdd)
    {
        // Check if this dish can add
        // i.e u cant add wanton to chicken rice lol
        if (CheckIfCanAdd(IngredientToAdd))
        {
            ListOfSubIngredients.Add(IngredientToAdd);

            return true;
        }

        return false;
    }

    /// <summary>
    /// For setting up the sprites after generating 
    /// </summary>
    public void SetUpSprite()
    {

        foreach (SubIngredient ingredient in ListOfSubIngredients)
        {
            int index = 0;
            
            // Check the dictionary if the sub ingredient is inside
            // If it is then get the index related to the sub ingredient
            // then enable the sprite
            if (ChildSprites.TryGetValue(ingredient, out index))
            {
                ChildObjects[index].SetActive(true);
            }
        }

        //foreach (KeyValuePair<SubIngredient, int> entry in ChildSprites)
        //{
        //    ChildObjects[entry.Value].SetActive(true);
        //}
    }

    public bool CheckIfCanAdd(SubIngredient IngredientToAdd)
    {
        int index = 0;

        if (ChildSprites.TryGetValue(IngredientToAdd, out index))
        {
            return true;
        }

        return false;
    }

    #endregion

    public virtual void ToggleSprites(IngredientObject newIngredient)
    {
        // Use c# dictionary
        // Make a mini class that handles adding to dictionary
        // Use the enum as a key and the sprite as the value
        // poo poo pee pee

        // Testing purposes
        // When finalise it when some art is inside
        if (newIngredient.subIngredient == SubIngredient.RoastChicken)
        {
            ChildObjects[0].SetActive(true);
        }
    }


}

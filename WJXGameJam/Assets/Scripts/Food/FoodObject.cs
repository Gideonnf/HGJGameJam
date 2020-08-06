using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class FoodObject : MonoBehaviour
{
    public FoodData m_FoodDate = new FoodData();

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

    DishDictionary DictionaryReference;

    void Awake()
    {
        DictionaryReference = GetComponent<DishDictionary>();
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

    private void OnDisable()
    {
         if (!FoodManager.m_ShuttingDown)
             FoodManager.Instance.RemoveFromPrepSlot(gameObject);

         ResetFood();
    }

    //TODO:: Food Object doesnt have checking for dropping on customers
    // Similar to how ingredient object drop on food objects


    // Food object snap position is also breaking because draggable object controller is detecting collision between itself

    private void Update()
    {
        //Debug.Log("Snap position of " + gameObject.name  + " : " + GetComponent<DraggableObjectController>().GetStartPos());

        // TESTING 
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // 
            //if (AddSubIngredient(SubIngredient.CharSiew))
            //    Debug.Log("Successfully Added");
            //else
            //    Debug.Log("Unsuccessful");

           // SetUpSprite();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (AddSubIngredient(SubIngredient.RoastChicken))
                Debug.Log("Successfully Added");
            else
                Debug.Log("Unsuccessful");
        }

        //TODO:: Pee Pee Poo Poo

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (AddSubIngredient(SubIngredient.RoastDuck))
                Debug.Log("Successfully Added");
            else
                Debug.Log("Unsuccessful");
        }

        //TODO:: Poop hehehehe

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
        if (m_FoodDate.foodType != IngredientToAdd.foodType)
            return false;

        // Add up the cost
        m_FoodDate.totalCost += IngredientToAdd.ingredientCost;


        // I just realise this has a kinda big issue lol
        // Right now i separate the noodle and chicken prefabs
        // they both have different main dish
        // the plate that is created has no main and sub ingredients until the user adds it
        // i need to find a way to make it so it'll change based on what main ingredient the user puts on the plate
        // fuk
        // i'll wait for both noodle and chicken to finish

        // If it is a main ingredient
        if (IngredientToAdd.IsMain)
        {
            // Check if it has no main ingredient yet
            if (m_FoodDate.mainIngredient == MainIngredient.NoIngredient)
            {
                // If there isnt then add it and return true
                m_FoodDate.mainIngredient = IngredientToAdd.mainIngredient;

                return true;
            }
        }   
        // It is a sub ingredient
        else
        {
            // Check if is a valid sub ingredient
            if(DictionaryReference.CheckForIngredient(IngredientToAdd.subIngredient))
            {
                // If check for conflict returns true means that the ingredient conflicts with the dish currently
                // Is it already inside?
                if (CheckIfCanAdd(IngredientToAdd.subIngredient)) 
                {
                    
                    m_FoodDate.ListOfSubIngredients.Add(IngredientToAdd.subIngredient);
                    return true;
                }
            }

            // Add it to the sub ingredient list
            //  m_FoodDate.ListOfSubIngredients.Add(IngredientToAdd.subIngredient);
        }

        m_FoodDate.totalCost -= IngredientToAdd.ingredientCost;

        return false;
    }

    #region For Randomisation
    // cuz need to manually add the ingredeitsn
    // laymao
    // poopiedy poo

    public bool AddMainIngredient(MainIngredient IngredientToAdd)
    {
        if (m_FoodDate.mainIngredient == MainIngredient.NoIngredient)
        {
            m_FoodDate.mainIngredient = IngredientToAdd;
            return true;  
        }

        return false;
    }

    public bool AddSubIngredient(SubIngredient IngredientToAdd)
    {
        // Check if this dish can add and check if it conflicts
        // i.e u cant add wanton to chicken rice lol
        if (DictionaryReference.CheckForIngredient(IngredientToAdd))
        {
            //if (!CheckForConflict(IngredientToAdd))
            {
                m_FoodDate.ListOfSubIngredients.Add(IngredientToAdd);

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// For setting up the sprites after generating 
    /// </summary>
    public void SetUpSprite()
    {
        if (m_FoodDate.mainIngredient != MainIngredient.NoIngredient)
        {
            Sprite mainSprite;

            // Check for which main ingredient did it add
            if (DictionaryReference.MainIngredientSprites.TryGetValue(m_FoodDate.mainIngredient, out mainSprite))
            {
                // if it successfully got a value, change the sprite

                ChildObjects[0].SetActive(true);

                // If the sprite is different
                // change it
                if (ChildObjects[0].GetComponent<SpriteRenderer>().sprite != mainSprite)
                    ChildObjects[0].GetComponent<SpriteRenderer>().sprite = mainSprite;
            }

            //ChildObjects[0].SetActive(true);
        }

        foreach (SubIngredient ingredient in m_FoodDate.ListOfSubIngredients)
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
        // if the food is already in the list of ingredients
        // return false cause u cant have 2 chicken
        // we not that atas yet
        if (m_FoodDate.ListOfSubIngredients.Contains(IngredientToAdd))
        {
            return false;
        }
        // if theres conflict
        else if (CheckForConflict(IngredientToAdd))
        {
            return false;
        }

        return true;
    }

    public void ResetFood()
    {
        m_FoodDate.totalCost = 0;

        //reset main ingredient
        m_FoodDate.mainIngredient = MainIngredient.NoIngredient;

        //reset subingredients
        m_FoodDate.ListOfSubIngredients.Clear();
        foreach (GameObject child in ChildObjects)
        {
            child.SetActive(false);
        }
    }

    #endregion

    /// <summary>
    /// returns true if there is conflict
    ///
    /// </summary>
    /// <param name="ingredientToAdd"> The ingredient that is being added</param>
    /// <returns></returns>
    public bool CheckForConflict(SubIngredient ingredientToAdd)
    {
        // Loop through the conflicting ingredient list
        foreach (KeyValuePair<SubIngredient, SubIngredient> entry in DictionaryReference.ConflictingSubIngredients)
        {
            if (entry.Key == ingredientToAdd || entry.Value == ingredientToAdd)
            {
                // If it contains the key or the value then return true
                if (m_FoodDate.ListOfSubIngredients.Contains(entry.Key) || m_FoodDate.ListOfSubIngredients.Contains(entry.Value))
                {
                    return true;
                }
            }
        }

        return false;
    }

}

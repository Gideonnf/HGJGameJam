using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishController : MonoBehaviour
{
    // Keeps track of what food it is
    public FoodObject foodObject;

    [Tooltip("Stores a list of all the children object")]
    // Keeps track of all the sub ingredients 
    // Allows for easy enabling and disabling of sub ingredients
    // Each sub ingredient is a new child object
    // The main object being the dish
    public List<GameObject> ChildObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        //foodObject = new FoodObject();

        // Loop through the child objects
        foreach(Transform child in transform)
        {
            // Add them to the list
            ChildObjects.Add(child.gameObject);
            // Set active to false
            child.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Call this function to add the ingredient object to the dish
    /// </summary>
    /// <param name="newIngredient">  </param>
    public bool AddToDish(IngredientObject newIngredient)
    {
        // Add it to the food object 
        if( foodObject.AddIngredient(newIngredient) )
        {
            // Toggle the sprite
            ToggleSprites(newIngredient);

            return true;
        }

        return false;
    }

    public virtual void ToggleSprites(IngredientObject newIngredient)
    {
        // Testing purposes
        // When finalise it when some art is inside
        if (newIngredient.subIngredient == SubIngredient.RoastChicken)
        {
            ChildObjects[0].SetActive(true);
        }
    }
}

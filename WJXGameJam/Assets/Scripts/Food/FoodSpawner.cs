using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IngredientSpawn
{
    [Tooltip("Spawn location for the food")]
    public Transform SpawnPosition;
    [Tooltip("If the spot is taken already or not")]
    public bool IsTaken;
}

public class FoodSpawner : MonoBehaviour
{
    [Tooltip("The tag to pool the object from the object pooler. pls dont typo")]
    public string ingredientTag;

    [Tooltip("Tag to the main dish object. (Spawns empty with only a plate)")]
    public string dishTag;

    //[Tooltip("List of all the spawn locations")]
    //public List<IngredientSpawn> spawns = new List<IngredientSpawn>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// The function for pulling the ingredient from the object pooler
    /// </summary>
    public void SpawnFoodIngredient()
    {
        // Pull the new ingredient
        // Temporarily spawning for testing purposes
        // GameObject newIngredient = ObjectPooler.Instance.SpawnFromPool(ingredientTag, this.transform.position, this.transform.rotation);
        GameObject newIngredient = ObjectPooler.Instance.FetchGO(ingredientTag);

        if (FoodManager.Instance.AddToPrepSlots(newIngredient, ingredientTag))
        {
            // If it added it to the list successfully
            // If it was successfully added
            newIngredient.GetComponent<IngredientObject>().foodTag = ingredientTag;

        }
        else
        {
            // it fuked up
            // set back to false
            newIngredient.SetActive(false);
            Debug.Log("no more space liao");

        }

        // Loop through the list
        //foreach (IngredientSpawn spawnPoints in spawns)
        //{
        //    // If the spawn point isn't taken yet
        //    if (spawnPoints.IsTaken == false)
        //    {
        //        // Spawn it there

        //        // Set boolean flag to true
        //        spawnPoints.IsTaken = true;
        //        break;
        //    }
        //}

        // if it reaches here then there is no available spawns
        return;
    }

    /// <summary>
    /// Spawns the main dishs
    /// The main dish spawns empty with only the base plate/what ever is used as the base
    /// </summary>
    public void SpawnMainDish()
    {
        //GameObject mainDish = ObjectPooler.Instance.SpawnFromPool(dishTag, this.transform.position, this.transform.rotation);
        GameObject mainDish = ObjectPooler.Instance.FetchGO(dishTag);

        // Add the main dish
        if (FoodManager.Instance.AddToPrepSlots(mainDish, dishTag))
        {
            // set the food tag to the same as dish tag
            // dish tag is used to pull from object pooler and also as a key to keep track
            mainDish.GetComponent<FoodObject>().m_FoodDate.foodTag = dishTag;
            // If it was successfully added
            //mainDish.GetComponent<IngredientObject>().foodTag = dishTag;
            // Return if it works
            return;
        }
        else
        {
            // If not then that means that its still full and it is to set active to false
            //mainDish.SetActive(false);
            Debug.Log("no more space liao");
        }

    }
}

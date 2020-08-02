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

    [Tooltip("List of all the spawn locations")]
    public List<IngredientSpawn> spawns = new List<IngredientSpawn>();


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
        GameObject newIngredient = ObjectPooler.Instance.SpawnFromPool(ingredientTag, this.transform.position, this.transform.rotation);

        // Loop through the list
        foreach (IngredientSpawn spawnPoints in spawns)
        {
            // If the spawn point isn't taken yet
            if (spawnPoints.IsTaken == false)
            {
                // Spawn it there

                // Set boolean flag to true
                spawnPoints.IsTaken = true;
                break;
            }
        }

        // if it reaches here then there is no available spawns
        return;
    }

    /// <summary>
    /// Spawns the main dishs
    /// The main dish spawns empty with only the base plate/what ever is used as the base
    /// </summary>
    public void SpawnMainDish()
    {
        GameObject mainDish = ObjectPooler.Instance.SpawnFromPool(dishTag, this.transform.position, this.transform.rotation);

        // Add the main dish
        if (FoodManager.Instance.AddToPrepSlots(mainDish))
        {
            // If it was successfully added

            // Return if it works
            return;
        }
        else
        {
            // If not then that means that its still full and it is to set active to false
            mainDish.SetActive(false);
        }

    }
}

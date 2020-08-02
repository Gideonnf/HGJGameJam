using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawn
{
    [Tooltip("Spawn location for the food")]
    public Transform SpawnPosition;
    [Tooltip("If the spot is taken already or not")]
    public bool IsTaken;
}

public class IngredientSpawner : MonoBehaviour
{
    [Tooltip("The tag to pool the object from the object pooler. pls dont typo")]
    public string ingredientTag;

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
}

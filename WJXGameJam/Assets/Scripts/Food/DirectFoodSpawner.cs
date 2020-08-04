using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectFoodSpawner : MonoBehaviour
{
    /// <summary>
    /// Adds ingredients directly to dishes
    /// Cause rice is a unique special snow flake lol
    /// </summary>

    [Tooltip("The tag for the ingredient to pull from object pooler")]
    public string ingredientTag;

    [Tooltip("The tag for target dish to interact with")]
    public string targetTag;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnRice()
    {
        GameObject RiceIngredient = ObjectPooler.Instance.FetchGO(ingredientTag);

        // if it added to the dish successfully
        if (FoodManager.Instance.AddToDish(RiceIngredient, targetTag))
        {
            RiceIngredient.SetActive(false);
        }
        else
        {
            RiceIngredient.SetActive(false);
        }
    }
}

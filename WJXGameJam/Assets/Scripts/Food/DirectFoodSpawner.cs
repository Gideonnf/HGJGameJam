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

    public string m_SoundName = "";

    public void SpawnRice()
    {
        GameObject RiceIngredient = ObjectPooler.Instance.FetchGO(ingredientTag);

        // if it added to the dish successfully
        if (FoodManager.m_ShuttingDown)
            return;

        if (FoodManager.Instance.AddToDish(RiceIngredient, targetTag))
        {
            RiceIngredient.SetActive(false);
            SoundManager.Instance.Play(m_SoundName);
        }
        else
        {
            RiceIngredient.SetActive(false);
        }
    }
}

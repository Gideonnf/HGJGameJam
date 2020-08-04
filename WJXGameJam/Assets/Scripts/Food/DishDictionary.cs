using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DishDictionary : MonoBehaviour
{
    FoodObject FoodObjectReference;

    public SubIngredient[] ListOfSubIngredients;

    public FoodStage currentStage;

    public Dictionary<SubIngredient, SubIngredient> ConflictingIngredients = new Dictionary<SubIngredient, SubIngredient>();

    [Header("Sprites for changing main ingredients")]
    public MainIngredient[] ListOfMainIngredients;

    public Sprite[] ListOfIngredientSprites;

    [NonSerialized]
    public Dictionary<MainIngredient, Sprite> MainIngredientSprites = new Dictionary<MainIngredient, Sprite>();

    // Start is called before the first frame update
    void Awake()
    {
        FoodObjectReference = GetComponent<FoodObject>();

        for(int i = 0; i < ListOfSubIngredients.Length; ++i)
        {
            // +1 because the first object in the child objects is for the main ingredient

            FoodObjectReference.ChildSprites.Add(ListOfSubIngredients[i], i + 1);
        }

        // For main dishes that shares the same base but different main ingredient
        // i.e Rice and Noodle on the same plate or different satay on the same plate
        for(int i = 0; i < ListOfMainIngredients.Length; ++i)
        {
            MainIngredientSprites.Add(ListOfMainIngredients[i], ListOfIngredientSprites[i]);
        }


        // i dont really want to do this
        // but i cant rlly think of a easier way to do this
        // cause of how some dishes have different ingredients but they take the same slot
        // mostly chinatown rice/noodle dish
        switch(currentStage)
        {
            case FoodStage.Chinatown:
                {
                    ConflictingIngredients.Add(SubIngredient.RoastPork, SubIngredient.Wanton);
                    break;
                }
        }


        //FoodObjectReference.ChildSprites.Add(SubIngredient.RoastChicken, 0);
        //FoodObjectReference.ChildSprites.Add(SubIngredient.RoastDuck, 1);
        //FoodObjectReference.ChildSprites.Add(SubIngredient.Wanton, 2);

    }

    public bool CheckForIngredient(SubIngredient ingredient)
    {
        // If it exist inside
        if (ListOfSubIngredients.Contains(ingredient))
            return true;

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

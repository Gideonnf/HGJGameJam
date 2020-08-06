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

    [Tooltip("For conflicting sub ingredients")]
    public Dictionary<SubIngredient, SubIngredient> ConflictingSubIngredients = new Dictionary<SubIngredient, SubIngredient>();

    [Tooltip("For sub ingredients that clashes with main ingredients")]
    public Dictionary<MainIngredient, SubIngredient> ConflictingMainIngredients = new Dictionary<MainIngredient, SubIngredient>();


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
                    ConflictingSubIngredients.Add(SubIngredient.RoastPork, SubIngredient.Wanton);
                    break;
                }
            case FoodStage.LittleIndia:
                {
                    // Thosai sauce cant add prata
                    ConflictingMainIngredients.Add(MainIngredient.ThreeSauces, SubIngredient.CheesePrata);
                    ConflictingMainIngredients.Add(MainIngredient.ThreeSauces, SubIngredient.EggPrata);
                    ConflictingMainIngredients.Add(MainIngredient.ThreeSauces, SubIngredient.OnionPrata);
                    ConflictingMainIngredients.Add(MainIngredient.ThreeSauces, SubIngredient.PlainPrata);

                    // Prata dish cant add thosai sauce
                    ConflictingMainIngredients.Add(MainIngredient.CurrySauce, SubIngredient.Thosai);

                    // U cant add thosai when u add pratas
                    ConflictingSubIngredients.Add(SubIngredient.Thosai, SubIngredient.CheesePrata);
                    ConflictingSubIngredients.Add(SubIngredient.Thosai, SubIngredient.EggPrata);
                    ConflictingSubIngredients.Add(SubIngredient.Thosai, SubIngredient.OnionPrata);
                    ConflictingSubIngredients.Add(SubIngredient.Thosai, SubIngredient.PlainPrata);
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

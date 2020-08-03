using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishDictionary : MonoBehaviour
{
    FoodObject FoodObjectReference;

    public SubIngredient[] ListOfSubIngredients;

    // Start is called before the first frame update
    void Start()
    {
        FoodObjectReference = GetComponent<FoodObject>();

        for(int i = 0; i < ListOfSubIngredients.Length; ++i)
        {
            FoodObjectReference.ChildSprites.Add(ListOfSubIngredients[i], i);
        }

        //FoodObjectReference.ChildSprites.Add(SubIngredient.RoastChicken, 0);
        //FoodObjectReference.ChildSprites.Add(SubIngredient.RoastDuck, 1);
        //FoodObjectReference.ChildSprites.Add(SubIngredient.Wanton, 2);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

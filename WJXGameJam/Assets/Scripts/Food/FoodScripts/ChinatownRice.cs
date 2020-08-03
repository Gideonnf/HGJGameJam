using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChinatownRice : MonoBehaviour
{

    FoodObject FoodObjectReference;

    // Start is called before the first frame update
    void Start()
    {
        FoodObjectReference = GetComponent<FoodObject>();

        // Add dis shit
        FoodObjectReference.ChildSprites.Add(SubIngredient.RoastChicken, 0);
        FoodObjectReference.ChildSprites.Add(SubIngredient.CharSiew, 1);
        FoodObjectReference.ChildSprites.Add(SubIngredient.RoastDuck, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChinatownNoodle : MonoBehaviour
{
    FoodObject FoodObjectReference;

    // Start is called before the first frame update
    void Start()
    {
        FoodObjectReference = GetComponent<FoodObject>();

        FoodObjectReference.ChildSprites.Add(SubIngredient.RoastChicken, 0);
        FoodObjectReference.ChildSprites.Add(SubIngredient.RoastDuck, 1);
        FoodObjectReference.ChildSprites.Add(SubIngredient.Wanton, 2);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

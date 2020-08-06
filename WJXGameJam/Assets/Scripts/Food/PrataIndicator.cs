using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrataIndicator : MonoBehaviour
{

    [HideInInspector]
    public GameObject IndicatorReference = null;

    IngredientObject ingredientObject;

    // Start is called before the first frame update
    void Start()
    {
        ingredientObject = GetComponent<IngredientObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IndicatorReference != null)
        {
            //if (ingredientObject.)
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PrataSprite
{
    public string name = "";

    public List<Sprite> ListOfSprites = new List<Sprite>();
}

public class PrataIndicator : MonoBehaviour
{

    [HideInInspector]
    public GameObject IndicatorReference = null;

    public List<Sprite> ListOfIndicatorSprites = new List<Sprite>();

    [Header("Prata Sprite Variants")]
    [Tooltip("AAAAAAAAAAAAAAA KMS")]
    public List<PrataSprite> PrataVariants = new List<PrataSprite>();

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
            IndicatorReference.SetActive(true);

            if (ingredientObject.subIngredient == SubIngredient.CheesePrata)
            {
                IndicatorReference.GetComponent<SpriteRenderer>().sprite = ListOfIndicatorSprites[0];
            }
            else if (ingredientObject.subIngredient == SubIngredient.EggPrata)
            {
                IndicatorReference.GetComponent<SpriteRenderer>().sprite = ListOfIndicatorSprites[1];

            }
            else if (ingredientObject.subIngredient == SubIngredient.OnionPrata)
            {
                IndicatorReference.GetComponent<SpriteRenderer>().sprite = ListOfIndicatorSprites[2];

            }
            else if (ingredientObject.subIngredient == SubIngredient.PlainPrata)
            {
                IndicatorReference.GetComponent<SpriteRenderer>().sprite = ListOfIndicatorSprites[3];

            }
        }
    }

    public void UpdateSubIngredient()
    {

    }
}

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

    FoodStateManager foodStateManager;

    SubIngredient currentSubIngredient;

   // FoodSpriteChanger spriteChanger;

    // Start is called before the first frame update
    void Start()
    {
        foodStateManager = GetComponent<FoodStateManager>();
        //spriteChanger = GetComponent<FoodSpriteChanger>();
        ingredientObject = GetComponent<IngredientObject>();
        currentSubIngredient = ingredientObject.subIngredient;
    }

    // Update is called once per frame
    void Update()
    {
        if (IndicatorReference != null)
        {
            if (IndicatorReference.activeSelf == false)
                IndicatorReference.SetActive(true);

            if (ingredientObject.subIngredient == SubIngredient.CheesePrata)
            {
                //Debug.Log("IS A CHEESE PRATA");
                IndicatorReference.GetComponent<SpriteRenderer>().sprite = ListOfIndicatorSprites[0];
            }
            else if (ingredientObject.subIngredient == SubIngredient.EggPrata)
            {
                //Debug.Log("IS A EGG PRATA");
                IndicatorReference.GetComponent<SpriteRenderer>().sprite = ListOfIndicatorSprites[1];

            }
            else if (ingredientObject.subIngredient == SubIngredient.OnionPrata)
            {
                //Debug.Log("IS A ONION PRATA");
                IndicatorReference.GetComponent<SpriteRenderer>().sprite = ListOfIndicatorSprites[2];

            }
            else if (ingredientObject.subIngredient == SubIngredient.PlainPrata)
            {
                IndicatorReference.GetComponent<SpriteRenderer>().sprite = ListOfIndicatorSprites[3];

            }
        }
    }

    public void UpdateIndicator()
    {

    }

    /// <summary>
    /// To reset the sprite changer sprites to the plain prata 
    /// </summary>
    public void ResetSprites()
    {
        for (int i = 0; i < PrataVariants[3].ListOfSprites.Count; ++i)
        {
            foodStateManager.foodStates[i + 2].FoodSprite = PrataVariants[3].ListOfSprites[i];
            //spriteChanger.spriteList[i + 2] = PrataVariants[3].ListOfSprites[i];
        }
    }


    public void UpdateSubIngredient()
    {
        if (foodStateManager == null)
            return;

        currentSubIngredient = ingredientObject.subIngredient;

        if (ingredientObject.subIngredient == SubIngredient.CheesePrata)
        {
            for(int i = 0; i < PrataVariants[0].ListOfSprites.Count; ++i)
            {
                SoundManager.Instance.Play("Cheese");
                foodStateManager.foodStates[i + 2].FoodSprite = PrataVariants[0].ListOfSprites[i];
               // spriteChanger.spriteList[i + 2] = PrataVariants[0].ListOfSprites[i];
            }
        }
        else if (ingredientObject.subIngredient == SubIngredient.EggPrata)
        {
            for (int i = 0; i < PrataVariants[1].ListOfSprites.Count; ++i)
            {
                SoundManager.Instance.Play("EggCrack");
                foodStateManager.foodStates[i + 2].FoodSprite = PrataVariants[1].ListOfSprites[i];
                //spriteChanger.spriteList[i + 2] = PrataVariants[1].ListOfSprites[i];
            }
        }
        else if (ingredientObject.subIngredient == SubIngredient.OnionPrata)
        {
            for (int i = 0; i < PrataVariants[2].ListOfSprites.Count; ++i)
            {
                SoundManager.Instance.Play("SprinkleOnion");
                foodStateManager.foodStates[i + 2].FoodSprite = PrataVariants[2].ListOfSprites[i];
                //spriteChanger.spriteList[i + 2] = PrataVariants[2].ListOfSprites[i];
            }
        }
    }
}

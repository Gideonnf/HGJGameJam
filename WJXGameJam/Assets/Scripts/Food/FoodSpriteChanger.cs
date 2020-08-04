﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class FoodSpriteChanger : MonoBehaviour
{
    [Tooltip("Sprites should be added in order of doneness")]
    public List<Sprite> spriteList = new List<Sprite>();

    private float[] cookingTimes;
    private IngredientObject IngredientRef;
    private float normalisedCookingTime = 0.0f;

    private int spriteStage = 0;

    // Start is called before the first frame update
    void Start()
    {
        IngredientRef = this.gameObject.GetComponent<IngredientObject>();
        cookingTimes = new float[spriteList.Count];

        for (int i = 0; i < cookingTimes.Length; ++i)
        {
            cookingTimes[i] = i * 1 / cookingTimes.Length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IngredientRef.isPreparing)
        {
            float tempNorm = IngredientRef.timeElapsed / IngredientRef.timeToPrepare;

            if (tempNorm >= cookingTimes[spriteStage])
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = spriteList[spriteStage];

                if (spriteStage + 1 < spriteList.Count)
                {
                    //TODO: OVERCOOK FUNCTIONS
                    ++spriteStage;
                }
            }
        }
    }
}

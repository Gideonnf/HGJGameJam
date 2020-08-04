using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class FoodSpriteChanger : MonoBehaviour
{
    [Tooltip("Sprites should be added in order of doneness\nDO NOT add the default sprite")]
    public List<Sprite> spriteList = new List<Sprite>();
    [Tooltip("A default sprite must be added")]
    public Sprite defaultSprite;

    private float[] cookingTimes;
    private IngredientObject IngredientRef;

    private int spriteStage = 0;

    // Start is called before the first frame update
    void OnEnable()
    {
        //setting default values
        this.GetComponent<SpriteRenderer>().sprite = defaultSprite;
        spriteStage = 0;

        IngredientRef = this.gameObject.GetComponent<IngredientObject>();
        cookingTimes = new float[spriteList.Count];

        for (int i = 0; i < cookingTimes.Length; ++i)
        {
            cookingTimes[i] = (i + 1) * 1 / (float)cookingTimes.Length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IngredientRef.isPreparing)
        {
            float tempNorm = IngredientRef.timeElapsed / IngredientRef.timeToPrepare;
            if (tempNorm > 1)
                tempNorm = 1;

            if (tempNorm >= cookingTimes[spriteStage])
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = spriteList[spriteStage];

                if (spriteStage == cookingTimes.Length - 2) //second last, dish is done
                    IngredientRef.isDone = true;

                if (spriteStage + 1 < spriteList.Count)
                {
                    //TODO: OVERCOOK FUNCTIONS
                    ++spriteStage;
                }
                else
                {
                    IngredientRef.isPreparing = false;
                }
            }
        }
    }
}

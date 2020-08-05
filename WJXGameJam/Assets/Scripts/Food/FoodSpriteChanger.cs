using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpriteChanger : MonoBehaviour
{
    [Tooltip("Sprites should be added in order of doneness\nDO NOT add the default sprite")]
    public List<Sprite> spriteList = new List<Sprite>();
    [Tooltip("A default sprite must be added")]
    public Sprite defaultSprite;

    [Header("For Flicker glow effect")]
    public Material m_FlickerGlowMaterial;
    public Vector2 m_MinMaxGlowOpacity = new Vector2(0.0f, 1.0f);
    public float m_FlickerSpeed = 1.0f;
    Material m_CurrMaterial;
    float m_CurrFlickerIntensity = 0.0f;
    bool m_FlickerIncrease = true;

    private float[] cookingTimes;
    private IngredientObject IngredientRef;

    private int spriteStage = 0;

    public void Awake()
    {
        if (m_FlickerGlowMaterial != null)
        {
            Material newMaterial = new Material(m_FlickerGlowMaterial);
            this.GetComponent<SpriteRenderer>().material = newMaterial;
            m_CurrMaterial = newMaterial;

            ResetFlicker();
        }
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        //setting default values
        this.GetComponent<SpriteRenderer>().sprite = defaultSprite;
        spriteStage = 0;

        ResetFlicker();

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
                {
                    IngredientRef.isDone = true;
                }

                if (spriteStage + 1 < spriteList.Count)
                {
                    //TODO: OVERCOOK FUNCTIONS
                    ++spriteStage;
                }
                else
                {
                    ResetFlicker();
                    IngredientRef.isPreparing = false;
                }
            }
        }

        if (IngredientRef.isDone && IngredientRef.isPreparing)
        {
            FlickerEffect();
        }
    }

    void FlickerEffect()
    {
        if (m_FlickerIncrease)
        {
            m_CurrFlickerIntensity += Time.deltaTime * m_FlickerSpeed;
            if (m_CurrFlickerIntensity > m_MinMaxGlowOpacity.y)
                m_FlickerIncrease = false;
        }
        else
        {
            m_CurrFlickerIntensity -= Time.deltaTime * m_FlickerSpeed;

            if (m_CurrFlickerIntensity < m_MinMaxGlowOpacity.x)
                m_FlickerIncrease = true;
        }

        m_CurrMaterial.SetFloat("_FlickerIntensity", m_CurrFlickerIntensity);
    }

    void ResetFlicker()
    {
        m_CurrFlickerIntensity = m_MinMaxGlowOpacity.x;
        m_FlickerIncrease = true;
        m_CurrMaterial.SetFloat("_FlickerIntensity", m_MinMaxGlowOpacity.x);
    }
}

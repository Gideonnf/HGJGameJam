using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodPreperationState
{
    Raw,
    Cooking,
    Cooked,
    Burnt
}

[Serializable]
public class FoodState
{
    [Tooltip("AAAAAAAAAAAAAAAA")]
    public string name = "State Name";

    [Tooltip("How long this state last")]
    public float StateTime = 0.0f;

    [Tooltip("Do u really need a tool tip for this")]
    public Sprite FoodSprite;

    public FoodPreperationState foodPrepState;

}

public class FoodStateManager : MonoBehaviour
{
    ParticleSystem particleSystem;

    [Header("Food State Settings")]
    [Tooltip("poopy")]
    public List<FoodState> foodStates = new List<FoodState>();
    [Tooltip("If the dish can be burnt")]
    public bool CanBurn = false;
    [Tooltip("The sprite it will always appear at its beginning state")]
    public Sprite DefaultSprite = null;

    [Header("For Flicker glow effect")]
    public Material m_FlickerGlowMaterial;
    public Vector2 m_MinMaxGlowOpacity = new Vector2(0.0f, 1.0f);
    public float m_FlickerSpeed = 1.0f;
    Material m_CurrMaterial;
    float m_CurrFlickerIntensity = 0.0f;
    bool m_FlickerIncrease = true;

    [Header("Sounds")]
    public string m_OverCookSound = "";
    public string m_ReadySound = "";
    public string m_SoundInBetweenStage = "";

    [HideInInspector]
    public int currentStateIndex = 0;

    private IngredientObject ingredientRef;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (m_FlickerGlowMaterial != null)
        {
            Material newMaterial = new Material(m_FlickerGlowMaterial);
            this.GetComponent<SpriteRenderer>().material = newMaterial;
            m_CurrMaterial = newMaterial;

            ResetFlicker();
        }

        ingredientRef = GetComponent<IngredientObject>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (DefaultSprite != null)
            GetComponent<SpriteRenderer>().sprite = DefaultSprite;

        if (this.transform.GetComponentInChildren<ParticleSystem>() != null)
        {
            particleSystem = this.transform.GetComponentInChildren<ParticleSystem>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // start is a naughty boy
        // it almost never fuking works now for some reason
        // idk if it'll break for this but im not taking my chances
    }

    private void OnEnable()
    {
        // When its enabled 
        // reset the sprite to default and reset the state index
        if (DefaultSprite != null)
            spriteRenderer.sprite = DefaultSprite;

        currentStateIndex = 0;

        ResetFlicker();

        if (particleSystem)
            particleSystem.Pause();

    }

    // Update is called once per frame
    void Update()
    {

        // For any continuous updates that is for specific food states
        // mostly gonna be used for flicker effect
        if (foodStates[currentStateIndex].foodPrepState == FoodPreperationState.Cooked)
        {
            // for any thing else that is not burn specific
            FlickerEffect();
        }
        else if (foodStates[currentStateIndex].foodPrepState == FoodPreperationState.Burnt)
        {
            // its burnt
            ResetFlicker();
            // can do stuff like activate smoking shit
        }


        // Debug.Log("Food state lapsed : " + currentStateIndex);

        // If it reaches state time, means its time to change to next sprite state
        if (ingredientRef.timeElapsed >= foodStates[currentStateIndex].StateTime)
        {
            if (currentStateIndex + 1 >= foodStates.Count)
                return;
            // Increase state index at the end
            currentStateIndex++;

            // Reset time elapsed
            ingredientRef.timeElapsed = 0.0f;

            // Change the sprite
            spriteRenderer.sprite = foodStates[currentStateIndex].FoodSprite;


            // These only trigger once upon state changing
            // for continous update is above

            // Plays any sound that goes inbetween food states
            //chop chop
            SoundManager.Instance.Play(m_SoundInBetweenStage);


            // If it is at it's cooked state
            if (foodStates[currentStateIndex].foodPrepState == FoodPreperationState.Cooked)
            {
                // if its burnable means that this isn't its last state
                // it is still preparing
                if (!CanBurn)
                {
                    // if it cant burn then preparing will end on this state
                    ingredientRef.isPreparing = false;
                }

                ingredientRef.isDone = true;

                // for any thing else that is not burn specific
                FlickerEffect();
                SoundManager.Instance.Play(m_ReadySound);


            }
            else if (foodStates[currentStateIndex].foodPrepState == FoodPreperationState.Burnt)
            {

                if(particleSystem)
                    particleSystem.Play();

                SoundManager.Instance.Play(m_OverCookSound);
            }
            else if (foodStates[currentStateIndex].foodPrepState == FoodPreperationState.Cooking)
            {

            }



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

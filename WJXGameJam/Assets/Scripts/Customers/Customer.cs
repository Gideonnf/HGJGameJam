using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [Header("Default Data")]
    public float m_WalkSpeed = 5.0f;
    public float m_PatienceTime = 3.0f;
    [Tooltip("The min multiplier amt for the number of dishes to appear")]
    public float[] m_DishesChances;
    [Tooltip("The max chance amt for each number of dishes")]
    public float[] m_MaxChance;

    [Header("Animations")]
    Animator m_Animator;
    public float m_WalkingAnimationSpeed = 1.5f;

    [Header("NPC expressions")]
    public List<CustomerMood> m_CustomerMoodDataList = new List<CustomerMood>();
    public SpriteRenderer m_FacialExpressionSpriteRenderer;
    CustomerExpressions m_CurrMood = CustomerExpressions.HAPPY;

    [Header("Updated Data")]
    float m_UpdatedWalkSpeed = 0.0f;
    float m_UpdatedPatienceTime = 0.0f;
    int m_NumberOfDishesToOrder = 1;

    [Header("Food")]
    public List<FoodObject> m_FoodOrders = new List<FoodObject>();
    List<FoodData> m_AvailableFoodRecipes = new List<FoodData>(); //available recipes this round

    float m_PatienceTimeTracker = 0.0f;

    Vector2 m_QueuePos = Vector2.zero;
    bool m_AtQueuePos = false;

    Vector2 m_ExitPos = Vector2.zero;

    Vector2 m_WalkDir = Vector2.zero;
    [HideInInspector] public bool m_LeavingStall = false;

    public delegate void OnLeftStall();
    public OnLeftStall OnLeftStallCallback;

    //voice over data
    [Header("Voice Overs")]
    public float m_ChanceToSaySpeech = 0.6f;
    VoiceOverManager m_VoiceOver = new VoiceOverManager();
    bool isMale = true;
    VoiceLanguages m_Language = VoiceLanguages.ENGLISH;

    //Food details
    FoodStage m_CurrFoodStage = FoodStage.Chinatown;

    public void Awake()
    {
        m_VoiceOver.Init(GetComponent<AudioSource>());
        m_Animator = GetComponent<Animator>();
    }

    public void SetFoodStage(FoodStage foodStage)
    {
        m_CurrFoodStage = foodStage;

        if (!FoodManager.m_ShuttingDown)
            m_AvailableFoodRecipes = FoodManager.Instance.GetFoodRecipesInStage(foodStage);
    }

    public void Init(float difficultyMultiplier, Vector2 spawnPos, Vector2 queuePos, Vector2 exitPos)
    {
        transform.position = new Vector3(spawnPos.x, spawnPos.y, transform.position.z);
        m_QueuePos = queuePos;
        m_ExitPos = exitPos;

        //reset variables
        m_PatienceTimeTracker = 0.0f;
        m_AtQueuePos = false;
        m_WalkDir = (queuePos - spawnPos).normalized;
        m_LeavingStall = false;

        //reset animations
        WalkingAnimation(true);
        m_Animator.speed = m_WalkingAnimationSpeed * (1.0f + difficultyMultiplier);

        //reset espressions
        m_CurrMood = CustomerExpressions.HAPPY;
        m_FacialExpressionSpriteRenderer.sprite = m_CustomerMoodDataList[(int)m_CurrMood].m_FacialExpressionSprite;

        //pick voice
        m_VoiceOver.PickVoice(false);

        //update the variables based on the multiplier
        m_UpdatedPatienceTime = m_PatienceTime * (1.0f - difficultyMultiplier);
        m_UpdatedWalkSpeed = m_WalkSpeed * (1.0f + difficultyMultiplier);
        m_NumberOfDishesToOrder = DecideFoodNumber(difficultyMultiplier);

        foreach (FoodObject foodObj in m_FoodOrders)
        {
            foodObj.ResetFood();
            foodObj.gameObject.SetActive(false);
        }

        CreateFoodOrder();
    }

    //decide number of food to order base on difficulty
    public int DecideFoodNumber(float difficultyMultiplier)
    {
        //return the number of dishes
        for (int i = m_DishesChances.Length - 1; i > 0; --i)
        {
            float chance = 0.0f;

            if (difficultyMultiplier >= m_DishesChances[i])
            {
                chance = EaseInOut(difficultyMultiplier);
                if (chance > m_MaxChance[i])
                    chance = m_MaxChance[i];

                if (chance >= Random.Range(0.0f, 1.0f))
                    return i + 1; 
            }
        }

        return 1;
    }

    public float EaseInOut(float value)
    {
        float end = 1.0f;
        float start = 0.0f;

        value /= .5f;
        end -= start;
        if (value < 1) return end * 0.5f * value * value + start;
        value--;
        return -end * 0.5f * (value * (value - 2) - 1) + start;
    }

    public void CreateFoodOrder()
    {

        for (int i = 0; i < m_NumberOfDishesToOrder; ++i)
        {
            switch (m_CurrFoodStage)
            {
                case FoodStage.Chinatown:
                    {
                        FoodData foodData = new FoodData();
                        foodData.mainIngredient = (MainIngredient)(Random.Range((int)MainIngredient.Rice, (int)MainIngredient.Noodle + 1));

                        CreateFood(foodData);
                    }
                    break;
                case FoodStage.GeylangSerai:
                    {

                    }
                    break;
            }
        }
    }

    public void CreateFood(FoodData foodData)
    {
        string testFoodRecipe = ""; //TODO:: REMOVE DEBUG
        testFoodRecipe += foodData.mainIngredient + " ";

        //randomize the number of sub ingredients
        foreach (FoodData foodRecipe in m_AvailableFoodRecipes)
        {
            if (foodRecipe.mainIngredient != foodData.mainIngredient)
                continue;

            List<SubIngredient> tempSubIngredientList = new List<SubIngredient>(foodRecipe.ListOfSubIngredients);
            
            int maxNumberOfIngredients = tempSubIngredientList.Count;
            int numberOfIngredients = Random.Range(1, maxNumberOfIngredients + 1);

            Debug.Log("ingredient number: " + numberOfIngredients);

            for (int i = 0; i < numberOfIngredients; ++i)
            {
                SubIngredient subIngredient = tempSubIngredientList[Random.Range(0, tempSubIngredientList.Count)];

                foodData.ListOfSubIngredients.Add(subIngredient);
                tempSubIngredientList.Remove(subIngredient);

                testFoodRecipe += subIngredient + " ";
            }
        }

        Debug.Log("recipe: " + testFoodRecipe);

        //add subingredients to the food object
        foreach (FoodObject foodObj in m_FoodOrders)
        {
            if (foodObj == null)
                continue;

            if (foodObj.gameObject.activeSelf)
                continue;

            foodObj.gameObject.SetActive(true); //set food active
            foodObj.AddMainIngredient(foodData.mainIngredient); //add main ingredient

            foreach (SubIngredient subIngredient in foodData.ListOfSubIngredients)
            {
                foodObj.AddSubIngredient(subIngredient);
            }

            foodObj.SetUpSprite(); //update the sprite accordingly
            break;
        }
    }

    public void Update()
    {
        //for when leaving the stall
        if (m_LeavingStall)
        {
            WalkAway();
            return;
        }

        //when customer walking to the queue slot
        if (!m_AtQueuePos)
        {
            WalkToQueue(); //walk to queue
            return;
        }

        //TODO:: have own sprite states for impatient, unhappy, about to walk away, and walking away
        m_PatienceTimeTracker += Time.deltaTime;
        UpdateExpression();
        if (m_PatienceTimeTracker > m_UpdatedPatienceTime)
        {
            LeavingStall();
            playerData.ReduceLives(); //reduce lives
        }
    }

    public void WalkToQueue()
    {
        transform.position += (Vector3)(m_WalkDir * Time.deltaTime * m_UpdatedWalkSpeed);

        //check if it overshot the position
        Vector2 nextDir = m_QueuePos - (Vector2)transform.position;
        nextDir.Normalize();
        if (Vector2.Dot(nextDir, m_WalkDir) < 0)
        {
            WalkingAnimation(false);
            m_AtQueuePos = true;
            transform.position = new Vector3(m_QueuePos.x, m_QueuePos.y, transform.position.z);

            PlayVoice(VoiceActions.GREETING);
        }
    }

    public void LeavingStall()
    {
        m_LeavingStall = true;
        m_WalkDir = (m_ExitPos - (Vector2)transform.position).normalized;
        WalkingAnimation(true);

        if (m_CurrMood == CustomerExpressions.HAPPY)
            PlayVoice(VoiceActions.SATISFIED);
        else
            PlayVoice(VoiceActions.ANGRY_LEAVE);
    }

    public void WalkAway()
    {
        //walk away to one side of the screen
        transform.position += (Vector3)(m_WalkDir * Time.deltaTime * m_UpdatedWalkSpeed);

        //check if it overshot the position
        Vector2 nextDir = m_ExitPos - (Vector2)transform.position;
        nextDir.Normalize();
        if (Vector2.Dot(nextDir, m_WalkDir) < 0)
        {
            WalkingAnimation(false);
            gameObject.SetActive(false);
            OnLeftStallCallback.Invoke();
        }
    }

    public bool CheckFood(FoodObject receivingFood)
    {
        //check if food is correct, walk away accordingly
        //if food correct pay, if not correct, pay only like a certain percentage of it
        //add to the customer serve
        //deduct the timer

        //only leave stall once all order is fufilled

        //check food object with customer's food object

        bool foundMatch = false;
        if (m_FoodOrders[0] != null)
        {
            if (m_FoodOrders[0].gameObject.activeSelf) //check if active first
            {
                if (m_FoodOrders[0].m_FoodDate.mainIngredient == receivingFood.m_FoodDate.mainIngredient)
                {
                    if (m_FoodOrders[0].m_FoodDate.ListOfSubIngredients.Count == receivingFood.m_FoodDate.ListOfSubIngredients.Count)
                    {
                        for (int i = 0; i < m_FoodOrders[0].m_FoodDate.ListOfSubIngredients.Count; ++i)
                        {
                            if (receivingFood.m_FoodDate.ListOfSubIngredients.Contains(m_FoodOrders[0].m_FoodDate.ListOfSubIngredients[i]))
                            {
                                foundMatch = true;
                                m_FoodOrders[0].gameObject.SetActive(false); //order fufilled
                            }
                            else
                                foundMatch = false;
                        }
                    }
                }
            }
        }

        if (m_FoodOrders[1] != null)
        {
            if (m_FoodOrders[1].gameObject.activeSelf) //check if active first
            {
                if (m_FoodOrders[1].m_FoodDate.mainIngredient == receivingFood.m_FoodDate.mainIngredient)
                {
                    if (m_FoodOrders[1].m_FoodDate.ListOfSubIngredients.Count == receivingFood.m_FoodDate.ListOfSubIngredients.Count)
                    {
                        for (int i = 0; i < m_FoodOrders[1].m_FoodDate.ListOfSubIngredients.Count; ++i)
                        {
                            if (receivingFood.m_FoodDate.ListOfSubIngredients.Contains(m_FoodOrders[1].m_FoodDate.ListOfSubIngredients[i]))
                            {
                                foundMatch = true;
                                m_FoodOrders[1].gameObject.SetActive(false); //order fufilled
                            }
                            else
                                foundMatch = false;
                        }
                    }
                }
            }
        }

        //order perfectly made, leave with a happy expression
        if (foundMatch)
        {
            //TODO:: pause the thing for a while, before walking away, show money sign
            ChangeExpression(CustomerExpressions.HAPPY);
            LeavingStall();
        }

        return foundMatch;
    }

    public void UpdateExpression()
    {
        int nextMood = (int)m_CurrMood + 1;
        //reach max angryness already
        if (nextMood >= m_CustomerMoodDataList.Count)
            return;

        //check current time to updated patience time
        float timeUsedPercentage = m_PatienceTimeTracker / m_UpdatedPatienceTime;
        //check if it change
        if (timeUsedPercentage > m_CustomerMoodDataList[(int)nextMood].m_MinPercentageForExpression)
        {
            m_CurrMood = (CustomerExpressions)nextMood;
            m_FacialExpressionSpriteRenderer.sprite = m_CustomerMoodDataList[(int)m_CurrMood].m_FacialExpressionSprite;

            //update speech
            if (m_CurrMood == CustomerExpressions.GETTING_IMPATIENT)
                PlayVoice(VoiceActions.GETTING_IMPATIENT);
            else if (m_CurrMood == CustomerExpressions.ANGRY)
                PlayVoice(VoiceActions.ANGRY);
        }
    }

    public void PlayVoice(VoiceActions voiceAction)
    {
        //play on chance
        float chance = Random.Range(0.0f,1.0f);
        if (m_ChanceToSaySpeech > chance)
            return;

        m_VoiceOver.PlayCustomerVoice(voiceAction);
    }

    public void ChangeExpression(CustomerExpressions expression)
    {
        m_CurrMood = expression;
        m_FacialExpressionSpriteRenderer.sprite = m_CustomerMoodDataList[(int)m_CurrMood].m_FacialExpressionSprite;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //CheckFood();
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        //CheckFood();
    }

    public void WalkingAnimation(bool walking)
    {
        if (m_Animator == null)
            return;

        m_Animator.SetBool("Walking", walking);
    }
}

[System.Serializable]
public class CustomerMood
{
    public CustomerExpressions m_Expressions = CustomerExpressions.HAPPY;
    [Tooltip("The min percentage of the total patience time for this expression")]
    [Range(0.0f, 1.0f)]
    public float m_MinPercentageForExpression = 0.0f;

    public Sprite m_FacialExpressionSprite;
}

public enum CustomerExpressions
{
    HAPPY,
    GETTING_IMPATIENT,
    ANGRY
}
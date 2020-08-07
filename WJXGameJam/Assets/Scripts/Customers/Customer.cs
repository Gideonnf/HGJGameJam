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
    public SpriteRenderer m_FacialExpressionSpriteRenderer;
    CustomerExpressions m_CurrMood = CustomerExpressions.HAPPY;

    [Header("NPC info")]
    public SpriteRenderer m_NPCSpriteRenderer;

    [Header("Updated Data")]
    float m_UpdatedWalkSpeed = 0.0f;
    float m_UpdatedPatienceTime = 0.0f;
    int m_NumberOfDishesToOrder = 1;

    [Header("Food")]
    public List<FoodObject> m_FoodOrders = new List<FoodObject>();
    public GameObject m_FoodOrderingUI;
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
    bool m_IsMale = true;
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

    public void Init(float difficultyMultiplier, Vector2 spawnPos, Vector2 queuePos, Vector2 exitPos, Sprite npcSprite, bool isMale = true, VoiceLanguages language = VoiceLanguages.ENGLISH)
    {
        transform.position = new Vector3(spawnPos.x, spawnPos.y, transform.position.z);
        m_QueuePos = queuePos;
        m_ExitPos = exitPos;

        //reset variables
        m_PatienceTimeTracker = 0.0f;
        m_AtQueuePos = false;
        m_WalkDir = (queuePos - spawnPos).normalized;
        m_LeavingStall = false;

        //change NPC sprite
        if (m_NPCSpriteRenderer != null)
            m_NPCSpriteRenderer.sprite = npcSprite;

        //reset animations
        if (m_Animator != null)
        {
            m_Animator.Play("Default");
            WalkingAnimation(true);
            m_Animator.speed = m_WalkingAnimationSpeed * (1.0f + difficultyMultiplier);
            m_Animator.SetInteger("CustomerState", (int)m_CurrMood);
        }

        //reset espressions
        ChangeExpression(CustomerExpressions.HAPPY);

        //pick voice
        m_IsMale = isMale;
        m_VoiceOver.PickVoice(isMale, language);

        //update the variables based on the multiplier
        m_UpdatedPatienceTime = m_PatienceTime * (1.0f - difficultyMultiplier);
        m_UpdatedWalkSpeed = m_WalkSpeed * (1.0f + difficultyMultiplier);
        m_NumberOfDishesToOrder = DecideFoodNumber(difficultyMultiplier);

        foreach (FoodObject foodObj in m_FoodOrders)
        {
            foodObj.ResetFood();
            foodObj.gameObject.SetActive(false);
        }

        if (m_FoodOrderingUI != null)
            m_FoodOrderingUI.SetActive(false);

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
                        FoodData foodData = new FoodData();
                        foodData.mainIngredient = (MainIngredient)(Random.Range((int)MainIngredient.GrillChicken, (int)MainIngredient.GrillBeef + 1));

                        CreateFood(foodData);
                    }
                    break;
                case FoodStage.LittleIndia:
                    {
                        FoodData foodData = new FoodData();
                        foodData.mainIngredient = (MainIngredient)(Random.Range((int)MainIngredient.CurrySauce, (int)MainIngredient.ThreeSauces + 1));

                        CreateFood(foodData);
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

            if (m_FoodOrderingUI != null)
                m_FoodOrderingUI.SetActive(true);
        }
    }

    public void LeavingStall()
    {
        m_LeavingStall = true;
        m_WalkDir = (m_ExitPos - (Vector2)transform.position).normalized;
        WalkingAnimation(true);

        if (m_CurrMood == CustomerExpressions.SATISFIED)
            PlayVoice(VoiceActions.SATISFIED);
        else
            PlayVoice(VoiceActions.ANGRY_LEAVE);

        if (m_FoodOrderingUI != null)
            m_FoodOrderingUI.SetActive(false);
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
            ChangeExpression(CustomerExpressions.SATISFIED);

            //add money to player and increase dish by 1
            playerData.moneyPerDay[playerData.moneyPerDay.Count - 1] += receivingFood.m_FoodDate.totalCost;
            ++playerData.dishesPerDay[playerData.dishesPerDay.Count - 1];

            Debug.Log(playerData.dishesPerDay[playerData.dishesPerDay.Count - 1]);
            Debug.Log(playerData.moneyPerDay[playerData.moneyPerDay.Count - 1]);

            LeavingStall();
            playerData.AddCustomerServedToDay();
        }

        return foundMatch;
    }

    public void UpdateExpression()
    {
        int nextMood = (int)m_CurrMood + 1;
        //reach max angryness already
        if (nextMood > (int)CustomerExpressions.ANGRY)
            return;

        //check current time to updated patience time
        float timeUsedPercentage = m_PatienceTimeTracker / m_UpdatedPatienceTime;
        //check if it change
        if (timeUsedPercentage > CustomerManager.Instance.m_CustomerSpriteData.m_CustomerMoodDataList[(int)nextMood].m_MinPercentageForExpression)
        {
            ChangeExpression((CustomerExpressions)nextMood);

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
        if (chance > m_ChanceToSaySpeech)
        {
            m_VoiceOver.PlayDefaultVoice(voiceAction, m_IsMale);
            return;
        }

        m_VoiceOver.PlayCustomerVoice(voiceAction);
    }

    public void ChangeExpression(CustomerExpressions expression)
    {
        m_CurrMood = expression;
        m_FacialExpressionSpriteRenderer.sprite = CustomerManager.Instance.m_CustomerSpriteData.GetCustomerFacialSprite((int)m_CurrMood);

        if (m_Animator != null)
            m_Animator.SetInteger("CustomerState", (int)m_CurrMood);
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

public enum CustomerExpressions
{
    HAPPY,
    GETTING_IMPATIENT,
    ANGRY,
    SATISFIED,
}
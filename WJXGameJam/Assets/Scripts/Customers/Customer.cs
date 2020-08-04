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

    [Header("Updated Data")]
    float m_UpdatedWalkSpeed = 0.0f;
    float m_UpdatedPatienceTime = 0.0f;
    int m_NumberOfDishesToOrder = 1;

    [Header("Food")]
    public List<FoodObject> m_FoodOrders = new List<FoodObject>();
    List<FoodData> m_AvailableFoodRecipes = new List<FoodData>();

    float m_PatienceTimeTracker = 0.0f;

    Vector2 m_QueuePos = Vector2.zero;
    bool m_AtQueuePos = false;

    Vector2 m_ExitPos = Vector2.zero;

    Vector2 m_WalkDir = Vector2.zero;
    bool m_LeavingStall = false;

    public delegate void OnLeftStall();
    public OnLeftStall OnLeftStallCallback;

    //Food details
    FoodStage m_CurrFoodStage = FoodStage.Chinatown;

    public void SetFoodStage(FoodStage foodStage)
    {
        m_CurrFoodStage = foodStage;

        m_AvailableFoodRecipes = FoodManager.Instance.GetFoodRecipesInStage(foodStage);
    }

    public void Init(float difficultyMultiplier, Vector2 spawnPos, Vector2 queuePos, Vector2 exitPos)
    {
        transform.position = spawnPos;
        m_QueuePos = queuePos;
        m_ExitPos = exitPos;

        m_PatienceTimeTracker = 0.0f;
        m_AtQueuePos = false;
        m_WalkDir = (queuePos - spawnPos).normalized;
        m_LeavingStall = false;

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

            //Debug.Log("ingredient number: " + numberOfIngredients);

            for (int i = 0; i < numberOfIngredients; ++i)
            {
                SubIngredient subIngredient = tempSubIngredientList[Random.Range(0, tempSubIngredientList.Count)];

                foodData.ListOfSubIngredients.Add(subIngredient);
                tempSubIngredientList.Remove(subIngredient);

                testFoodRecipe += subIngredient + " ";
            }
        }

        //Debug.Log("recipe: " + testFoodRecipe);

        //add subingredients to the food object
        foreach (FoodObject foodObj in m_FoodOrders)
        {
            if (foodObj == null)
                continue;

            if (foodObj.gameObject.activeSelf)
                continue;

            if (foodObj.m_FoodDate.mainIngredient != foodData.mainIngredient)
                continue;

            foodObj.gameObject.SetActive(true);

            foreach (SubIngredient subIngredient in foodData.ListOfSubIngredients)
            {
                foodObj.AddSubIngredient(subIngredient);
            }

            foodObj.SetUpSprite();
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
        if (m_PatienceTimeTracker > m_UpdatedPatienceTime)
        {
            LeavingStall();
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
            m_AtQueuePos = true;
            transform.position = m_QueuePos;
        }
    }

    public void LeavingStall()
    {
        m_LeavingStall = true;
        m_WalkDir = (m_ExitPos - (Vector2)transform.position).normalized;
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
            gameObject.SetActive(false);
            OnLeftStallCallback.Invoke();
        }
    }

    public void CheckFood()
    {
        //TODO:: check if food is correct, walk away accordingly
        //if food correct pay, if not correct, pay only like a certain percentage of it
        //add to the customer serve
        //deduct the timer
        //food will just disappear

        //only leave stall once all order is fufilled
        LeavingStall();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //CheckFood();
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        //CheckFood();
    }
}

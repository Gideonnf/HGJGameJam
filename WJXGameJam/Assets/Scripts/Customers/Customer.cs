﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [Header("Default Data")]
    public float m_WalkSpeed = 5.0f;
    public float m_PatienceTime = 3.0f;

    //public
    //TODO:: min and max number of dishes
    //TODO:: percentage chances of getting more than 1 dish

    [Header("Updated Data")]
    public float m_UpdatedWalkSpeed = 0.0f;
    public float m_UpdatedPatienceTime = 0.0f;

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


        CreateFoodOrder();
    }

    public void CreateFoodOrder()
    {
        //TODO:: create food order
        switch (m_CurrFoodStage)
        {
            case FoodStage.Chinatown:
                {

                }
                break;
            case FoodStage.GeylangSerai:
                {

                }
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
        m_PatienceTimeTracker += Time.fixedDeltaTime;
        if (m_PatienceTimeTracker > m_UpdatedPatienceTime)
        {
            LeavingStall();
        }
    }

    public void WalkToQueue()
    {
        transform.position += (Vector3)(m_WalkDir * Time.fixedDeltaTime * m_UpdatedWalkSpeed);

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
        transform.position += (Vector3)(m_WalkDir * Time.fixedDeltaTime * m_UpdatedWalkSpeed);

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

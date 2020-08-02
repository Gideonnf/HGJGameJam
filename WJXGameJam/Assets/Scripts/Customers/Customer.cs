using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    float m_PatienceTime = 0.0f;
    float m_PatienceTimeTracker = 0.0f;

    public void Init(float patienceTime, Vector2 spawnPos)
    {
        m_PatienceTime = patienceTime;
        transform.position = spawnPos;

        m_PatienceTimeTracker = 0.0f;

        //TODO:: also init a walk position to go too

        CreateFoodOrder();
    }

    public void CreateFoodOrder()
    {
        //TODO:: create food order
    }

    public void Update()
    {
        m_PatienceTimeTracker += Time.fixedDeltaTime;

        //TODO:: have own sprite states for impatient, unhappy, about to walk away, and walking away


        if (m_PatienceTimeTracker > m_PatienceTime)
        {
            WalkAway();
        }
    }

    public void WalkAway()
    {
        //TODO:: walk away to one side of the screen
    }

    public void CheckFood()
    {
        //TODO:: check if food is correct, walk away accordingly
        //if food correct pay, if not correct, pay only like a certain percentage of it
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

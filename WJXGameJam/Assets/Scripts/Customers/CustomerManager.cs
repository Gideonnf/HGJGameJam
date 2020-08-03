using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [Header("Stage details")]
    public FoodStage m_CurrFoodStage = FoodStage.Chinatown;

    [Header("Difficulty ramp")]
    [Range(0.0f, 1.0f)]
    public float m_MaxDifficultyPercentage = 0.9f;
    [Tooltip("The max number of customer served before it reaches max difficulty")]
    public int m_MaxCustomerDifficulty = 50;
    float m_CurrDifficulty = 0.0f;

    //when customer spawn, make sure spawn them at the correct positions
    [Header("Customers Details")]
    public CustomerObjectPooler m_CustomerObjPooler = new CustomerObjectPooler();
    public Transform m_CustomerQueuePosParent = null;
    public Transform m_CustomerEnterExitPosParent = null;

    List<Transform> m_CustomerQueuePosList = new List<Transform>();
    List<Transform> m_CustomerEnterExitPosList = new List<Transform>();

    GameObject[] m_CustomerQueuing;
    int m_CurrentCustomersInQueue = 0;
    int m_MaxCustomerInQueue = 3;

    void Awake()
    {
        m_CustomerObjPooler.Init(m_CurrFoodStage);

        if (m_CustomerQueuePosParent != null)
        {
            foreach(Transform queuePos in m_CustomerQueuePosParent)
            {
                m_CustomerQueuePosList.Add(queuePos);
            }
        }

        if (m_CustomerEnterExitPosParent != null)
        {
            foreach (Transform enterExitPos in m_CustomerEnterExitPosParent)
            {
                m_CustomerEnterExitPosList.Add(enterExitPos);
            }
        }

        m_CurrDifficulty = 0.0f;

        //dealing with customer in queue
        m_MaxCustomerInQueue = m_CustomerQueuePosList.Count;
        m_CurrentCustomersInQueue = 0;

        m_CustomerQueuing = new GameObject[m_MaxCustomerInQueue];
        for (int i = 0; i < m_MaxCustomerInQueue; ++i) //make sure its fully null
        {
            m_CustomerQueuing[i] = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    ++testVariable;
        //    UpdateDifficultyPercentage();
        //}

        if (m_CurrentCustomersInQueue < m_MaxCustomerInQueue)
        {
            GetNewCustomerToQueue();
        }
    }

    //TODO:: call the difficulty percentage after successfuully serving customer
    public void UpdateDifficultyPercentage()
    {
        int currCustomerServed = 0;

        if (playerData.customersPerDay.Count > 0)
            currCustomerServed = playerData.customersPerDay[playerData.customersPerDay.Count - 1];

        m_CurrDifficulty = (float)currCustomerServed / (float)m_MaxCustomerDifficulty;

        if (m_CurrDifficulty > m_MaxDifficultyPercentage)
            m_CurrDifficulty = m_MaxDifficultyPercentage;
    }

    public void GetNewCustomerToQueue()
    {
        GameObject customerObj = m_CustomerObjPooler.GetCustomerFromPooler();
        if (customerObj == null)
            return;

        Customer customer = customerObj.GetComponent<Customer>();
        if (customer == null)
            return;

        //init customer positions
        if (m_CustomerEnterExitPosList.Count <= 0)
            return;

        Vector2 enterPos = m_CustomerEnterExitPosList[Random.Range(0, m_CustomerEnterExitPosList.Count)].position;
        Vector2 exitPos = m_CustomerEnterExitPosList[Random.Range(0, m_CustomerEnterExitPosList.Count)].position;

        //find place for the new customer
        for (int i = 0; i < m_MaxCustomerInQueue; ++i)
        {
            if (m_CustomerQueuing[i] != null)
                continue;

            //initialise queuePos
            Vector2 queuePos = Vector2.zero;
            if (i < m_CustomerQueuePosList.Count)
                queuePos = m_CustomerQueuePosList[i].position;

            customerObj.SetActive(true);
            customer.Init(m_CurrDifficulty, enterPos, queuePos, exitPos);
            customer.OnLeftStallCallback += CustomerLeave;

            m_CustomerQueuing[i] = customerObj;
            ++m_CurrentCustomersInQueue;

            break;
        }
    }

    public void CustomerLeave()
    {
        //check which customer is inactive
        for (int i =0; i < m_MaxCustomerInQueue; ++i)
        {
            if (m_CustomerQueuing[i] == null)
                continue;

            if (m_CustomerQueuing[i].activeSelf)
                continue;

            Customer customer = m_CustomerQueuing[i].GetComponent<Customer>();
            if (customer != null)
                customer.OnLeftStallCallback -= CustomerLeave; //unsubscribe the listener

            m_CustomerQueuing[i] = null;
            --m_CurrentCustomersInQueue;

            break;
        }
    }
}

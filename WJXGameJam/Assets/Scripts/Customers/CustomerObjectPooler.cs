using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomerObjectPooler
{
    public int m_InitialCustomerSpawn = 1;

    public GameObject m_CustomerPrefab = null;
    public GameObject m_CustomerParent = null;
    
    private List<GameObject> m_CustomerList = new List<GameObject>();
    private FoodStage m_CurrFoodStage = FoodStage.Chinatown;

    public void Init(FoodStage foodStage)
    {
        if (m_CustomerPrefab == null)
            return;

        m_CurrFoodStage = foodStage;
        SpawnCustomer();

        foreach (GameObject customerObj in m_CustomerList)
        {
            Customer custsomer = customerObj.GetComponent<Customer>();

            if (custsomer != null)
            {
                custsomer.SetFoodStage(m_CurrFoodStage);
            }
        }
    }

    public void SpawnCustomer()
    {
        for (int i = 0; i < m_InitialCustomerSpawn; ++i)
        {
            GameObject customerObj = GameObject.Instantiate(m_CustomerPrefab);

            if (m_CustomerParent != null)
                customerObj.transform.SetParent(m_CustomerParent.transform);

            Customer custsomer = customerObj.GetComponent<Customer>();
            if (custsomer != null)
                custsomer.SetFoodStage(m_CurrFoodStage);

            customerObj.transform.position = new Vector3(customerObj.transform.position.x, customerObj.transform.position.y, -(m_CustomerList.Count / 10.0f));
            customerObj.SetActive(false);

            m_CustomerList.Add(customerObj);
        }
    }

    public GameObject GetCustomerFromPooler()
    {
        foreach(GameObject customer in m_CustomerList)
        {
            if (!customer.activeSelf)
                return customer;
        }

        SpawnCustomer();

        return GetCustomerFromPooler();
    }
}

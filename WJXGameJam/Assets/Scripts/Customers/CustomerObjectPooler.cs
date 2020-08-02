using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomerObjectPooler
{
    public int m_InitialCustomerSpawn = 1;

    public GameObject m_CustomerPrefab = null;
    public GameObject m_CustomerParent = null;
    
    private List<GameObject> m_CustomerList = new List<GameObject>();

    public void Init()
    {
        if (m_CustomerPrefab == null)
            return;

        SpawnCustomer();
    }

    public void SpawnCustomer()
    {
        for (int i = 0; i < m_InitialCustomerSpawn; ++i)
        {
            GameObject customerObj = GameObject.Instantiate(m_CustomerPrefab);

            if (m_CustomerParent != null)
                customerObj.transform.SetParent(m_CustomerParent.transform);

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

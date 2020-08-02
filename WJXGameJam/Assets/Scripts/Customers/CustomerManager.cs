using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    //when customer spawn, make sure spawn them at the correct positions
    [Header("Customers")]
    public CustomerObjectPooler m_CustomerObjPooler = new CustomerObjectPooler();



    void Awake()
    {
        m_CustomerObjPooler.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class playerData
{
    public static List<float> moneyPerDay = new List<float>();
    public static List<int> dishesPerDay = new List<int>();
    public static List<int> customersPerDay = new List<int>();

    public static int Lives = 3;

    public static float GetTotalMoney()
    {
        var temptotal = 0.0f;
        foreach (var money in moneyPerDay)
        {
            temptotal += money;
        }
        return temptotal;
    }

    public static int GetTotalDishes()
    {
        var temptotal = 0;
        foreach (var dishes in dishesPerDay)
        {
            temptotal += dishes;
        }
        return temptotal;
    }

    public static int GetTotalCustomer()
    {
        int temptotal = 0;
        foreach (var customer in customersPerDay)
        {
            temptotal += customer;
        }
        return temptotal;
    }

    public static void ReduceLives()
    {
        --Lives;
        
        if (Lives <= 0)
        {
            //TODO:: gameover
        }
    }
}

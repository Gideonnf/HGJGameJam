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

    //TODO:: every start of the game will restart this
    public static void AddCustomerServedToDay()
    {
        if (customersPerDay.Count <= 0)
            return;

        customersPerDay[customersPerDay.Count - 1] += 1;

        //update the difficulty for endless run
        if (DataManager.Instance.isEndless)
            CustomerManager.Instance.UpdateDifficultyPercentage();
    }

    public static void ReduceLives()
    {
        --Lives;
        
        if (Lives <= 0)
        {
            //TODO:: gameover
        }
    }

    public static void ResetPlayerLives()
    {
        Lives = 3;
    }
}

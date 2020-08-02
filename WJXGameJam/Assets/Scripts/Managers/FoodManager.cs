using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

/// <summary>
/// A class that represent how much space there is to prepare dishes
/// 
/// </summary>
public class PreperationSlots
{
    // Stores a reference to the food object
    public GameObject FoodObject = null;

    // Boolean flag if the spot is taken
    public bool isTaken = false;
}

public class FoodManager : SingletonBase<FoodManager>
{
    

    // List of slots
    PreperationSlots[] prepSlots;

    [Tooltip("How many plates can they prepare at one time")]
    public int NumOfSlots = 1;

    [Tooltip("Stores the positions of where the plates are going to be at")]
    // idk how to name variables lol
    public Transform[] positionOfSlots;

    // Start is called before the first frame update
    void Start()
    {
        prepSlots = new PreperationSlots[NumOfSlots];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool AddToPrepSlots(GameObject ObjectToAdd)
    {
        // Loop it
        // Edit it
        // Return it
        for (int i = 0; i < prepSlots.Length; ++i)
        {
            if (prepSlots[i].isTaken == false)
            {
                // Store reference to the object
                prepSlots[i].FoodObject = ObjectToAdd;

                // Set the position
                ObjectToAdd.transform.position = positionOfSlots[i].position;

                // Set boolean to true
                prepSlots[i].isTaken = true;
                break;
            }
        }

        return false;
    }
}

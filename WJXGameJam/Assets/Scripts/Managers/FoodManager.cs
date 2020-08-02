using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

/// <summary>
/// A class that represent how much space there is to prepare dishes
/// 
/// </summary>
[System.Serializable]
public class PreperationSlots
{
    // Stores a reference to the food object
    public GameObject FoodObject = null;

    // Boolean flag if the spot is taken
    public bool isTaken = false;

    public PreperationSlots()
    {
        FoodObject = null;
        isTaken = false;
    }

   
}

public class FoodManager : SingletonBase<FoodManager>
{
    // List of slots
    List<PreperationSlots> prepSlots = new List<PreperationSlots>();

    [Tooltip("How many plates can they prepare at one time")]
    public int NumOfSlots = 1;

    [Tooltip("Stores the positions of where the plates are going to be at")]
    // idk how to name variables lol
    public Transform[] positionOfSlots;

    // Start is called before the first frame update
    void Start()
    {
       for (int i = 0; i < NumOfSlots; ++i)
        {
            PreperationSlots newSlot = new PreperationSlots();
            prepSlots.Add(newSlot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool TestingFunction(IngredientObject IngredientToAdd)
    {
        return prepSlots[0].FoodObject.GetComponent<DishController>().AddToDish(IngredientToAdd);
    }

    public bool AddToPrepSlots(GameObject ObjectToAdd)
    {
        // Loop it
        // Edit it
        // Return it
        for (int i = 0; i < prepSlots.Count; ++i)
        {
            if (prepSlots[i].isTaken == false)
            {
                // Store reference to the object
                prepSlots[i].FoodObject = ObjectToAdd;

                // Set the position
                ObjectToAdd.transform.position = positionOfSlots[i].position;

                // Set boolean to true
                prepSlots[i].isTaken = true;

                return true;
            }
        }

        return false;
    }
}

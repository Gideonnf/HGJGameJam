using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using UnityEditor;
using UnityEngine;

/// <summary>
/// A class that represent how much space there is to prepare dishes
/// 
/// </summary>
[Serializable]
public class PreperationSlots
{
    string name = "Perperation Slot";

    [Tooltip("Position of the object")]
    public Transform PositionOfSlot;

    [NonSerialized]
    // Stores a reference to the food object
    public GameObject FoodObject = null;

    [NonSerialized]
    // Boolean flag if the spot is taken
    public bool isTaken = false;

    public PreperationSlots()
    {
        FoodObject = null;
        isTaken = false;
    }
}

[Serializable]
public class PreperationList
{
    // Some stages has multiple ingredients that have different preperation spots
    // to make it easier u can create a list in teh food manager based on how many ingredients there are
    // poop pee boom

    [Tooltip("Name of the list")]
    public string name = "Name Of List";

    [Tooltip("Is the list for the base dish or an ingredient")]
    public bool isIngredient = false;

    [Tooltip("How many tags does this list handle")]
    // i.e chicken pork and duck shares the same preperation area
    // noodle and rice shares the same preperation area
    public string[] ListOfTags;

    [Tooltip("poop i want kms")]
    public List<PreperationSlots> prepSlots = new List<PreperationSlots>();
}

//[CustomEditor(typeof(PreperationList))]
//public class PreperationListEditor : Editor
//{
//    PreperationList PrepScript;
//    override public void OnInspectorGUI()
//    {
//        PrepScript = (PreperationList)target;

//        //TODO:: Commit seppuku maybe

//    }

//}

public class FoodManager : SingletonBase<FoodManager>
{
    // List of slots
    public List<PreperationList> ListOfPrepSlots = new List<PreperationList>();

    public List<FoodData> FoodReceipes = new List<FoodData>();
    

    //[Tooltip("How many plates can they prepare at one time")]
    //public int NumOfSlots = 1;

    //[Tooltip("Stores the positions of where the plates are going to be at")]
    //// idk how to name variables lol
    //public Transform[] positionOfSlots;

    // Start is called before the first frame update
    void Start()
    {
        //foreach (PreperationList PrepList in ListOfPrepSlots)
        //{
        //    for (int i = 0; i < PrepList.NumOfSlots)
        //    {
        //        PreperationSlots new
        //    }
        //}    

       //for (int i = 0; i < NumOfSlots; ++i)
       // {
       //     PreperationSlots newSlot = new PreperationSlots();
       //     prepSlots.Add(newSlot);
       // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool TestingFunction(IngredientObject IngredientToAdd)
    {
        return false;
       // return prepSlots[0].FoodObject.GetComponent<FoodObject>().AddToDish(IngredientToAdd);
    }

    //TODO:: Remove the object from the list when its done
    public bool RemoveFromPrepSlot(GameObject ObjectToRemove)
    {

        return false;
    }

    public bool AddToPrepSlots(GameObject ObjectToAdd, string FoodTag)
    {
        int listIndex = 0;

        // Get the list index
        // find out which list is for the food lol
        for (int i = 0; i < ListOfPrepSlots.Count; ++i)
        {
            if (ListOfPrepSlots[i].ListOfTags.Contains(FoodTag))
            {
                listIndex = i;
                break;
            }
        }

        // loop it
        // bop it
        // drop it
        // lock it
        for(int i = 0; i < ListOfPrepSlots[listIndex].prepSlots.Count; ++i)
        {
            if (ListOfPrepSlots[listIndex].prepSlots[i].isTaken == false)
            {
                ListOfPrepSlots[listIndex].prepSlots[i].FoodObject = ObjectToAdd;

                // Set the position
                ObjectToAdd.transform.position = ListOfPrepSlots[listIndex].prepSlots[i].PositionOfSlot.position;

                ListOfPrepSlots[listIndex].prepSlots[i].isTaken = true;

                return true;
            }
        }

        return false;

        //// Loop it
        //// Edit it
        //// Return it
        //for (int i = 0; i < prepSlots.Count; ++i)
        //{
        //    if (prepSlots[i].isTaken == false)
        //    {
        //        // Store reference to the object
        //        prepSlots[i].FoodObject = ObjectToAdd;

        //        // Set the position
        //        ObjectToAdd.transform.position = positionOfSlots[i].position;

        //        // Set boolean to true
        //        prepSlots[i].isTaken = true;

        //        return true;
        //    }
        //}

        //return false;
    }

    public List<FoodData> GetFoodRecipesInStage(FoodStage stage)
    {
        List<FoodData> foodRecipesInStage = new List<FoodData>();

        foreach (FoodData foodRecipe in FoodReceipes)
        {
            if (foodRecipe.foodStage == stage)
                foodRecipesInStage.Add(foodRecipe);
        }

        return foodRecipesInStage;
    }
}

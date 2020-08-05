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

    public void ResetSlot()
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

   // [Tooltip("Is the list for the base dish or an ingredient")]
    //public bool isIngredient = false;

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

       //TODO:: Commit seppuku maybe

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

    public bool RemoveFromPrepSlot(GameObject ObjectToRemove)
    {
        int listIndex = 0;
        string foodTag = "";

        // Check if the object is an ingredient object or food object
        if (ObjectToRemove.GetComponent<IngredientObject>())
        {
            foodTag = ObjectToRemove.GetComponent<IngredientObject>().foodTag;
        }
        else if (ObjectToRemove.GetComponent<FoodObject>())
        {
            foodTag = ObjectToRemove.GetComponent<FoodObject>().m_FoodDate.foodTag;
        }

        // Check if there is any list with the food tag
        for (int i = 0; i < ListOfPrepSlots.Count; ++i)
        {
            if (ListOfPrepSlots[i].ListOfTags.Contains(foodTag))
            {
                // if found, break the loop
                listIndex = i;
                break;
            }
        }

        if (listIndex >= ListOfPrepSlots.Count)
            return false;

        // loop through the list of prep slots
        for(int i = 0; i < ListOfPrepSlots[listIndex].prepSlots.Count; ++i)
        {
            // if we found the same object that is being removed
            if (ListOfPrepSlots[listIndex].prepSlots[i].FoodObject == ObjectToRemove)
            {
                // reset the slot
                ListOfPrepSlots[listIndex].prepSlots[i].FoodObject = null;
                ListOfPrepSlots[listIndex].prepSlots[i].isTaken = false;
            }
        }

        return false;
    }

    /// <summary>
    /// For directly adding ingredients to dishes
    /// </summary>
    /// <param name="IngredientToAdd"> ingredient GO that is being added </param>
    /// <param name="targetTag"> Tag of the target dish  they want to check</param>
    /// <returns></returns>
    public bool AddToDish(GameObject IngredientToAdd, string targetTag)
    {
        int listIndex = 0;

        for (int i = 0; i < ListOfPrepSlots.Count; ++i)
        {
            if (ListOfPrepSlots[i].ListOfTags.Contains(targetTag))
            {
                // if found, break the loop
                listIndex = i;
                break;
            }
        }

        IngredientObject ingredientObject = IngredientToAdd.GetComponent<IngredientObject>();

        // If adding a main iingredient
        if (ingredientObject.IsMain)
        {
            for (int i = 0; i < ListOfPrepSlots[listIndex].prepSlots.Count; ++i)
            {
                // It doesnt have it
                if (ListOfPrepSlots[listIndex].prepSlots[i].FoodObject == null)
                    continue;

                FoodObject foodObject = ListOfPrepSlots[listIndex].prepSlots[i].FoodObject.GetComponent<FoodObject>();

                //ListOfPrepSlots[listIndex].prepSlots[i].FoodObject.
                // If it has no current main ingredient
                if (foodObject.m_FoodDate.mainIngredient == MainIngredient.NoIngredient)
                {
                    // Change it to this one
                    foodObject.m_FoodDate.mainIngredient = ingredientObject.mainIngredient;

                    // toggle to sprite to show the main ingredient
                    foodObject.SetUpSprite();

                    return true;
                }
            }
        }
        else
        {
            // its a sub ingredient
            // idk if i need to do this yet
            for (int i = 0; i < ListOfPrepSlots[listIndex].prepSlots.Count; ++i)
            {
                // It doesnt have it
                if (ListOfPrepSlots[listIndex].prepSlots[i].FoodObject == null)
                    continue;

                //IngredientObject ingredientToAdd = ingredientObject

                FoodObject foodObject = ListOfPrepSlots[listIndex].prepSlots[i].FoodObject.GetComponent<FoodObject>();

                if (foodObject.AddIngredient(ingredientObject))
                {
                    // it was added
                    // Set up the sprite
                    foodObject.SetUpSprite();

                    return true;
                }

                // If it reaches here means that dish cannot add so loop till we find a food object that can
                // if it can't this function wil lreturn false
            }

        }
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

                // If it has a draggable component
                // set the start pos
                if (ObjectToAdd.GetComponent<DraggableObjectController>())
                    ObjectToAdd.GetComponent<DraggableObjectController>().SetStartPos(ObjectToAdd.transform.position);

                ListOfPrepSlots[listIndex].prepSlots[i].isTaken = true;

                return true;
            }
        }

        return false;

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

    public void ResetPrepList()
    {
        for (int i = 0; i < ListOfPrepSlots.Count; ++i)
        {
            for(int j = 0; j < ListOfPrepSlots[i].prepSlots.Count; ++j)
            {
                GameObject foodObject = ListOfPrepSlots[i].prepSlots[j].FoodObject;

                if (foodObject.GetComponent<FoodObject>())
                {
                    // If its a main food object
                    // i.e main dish
                    foodObject.GetComponent<FoodObject>().ResetFood();
                }
                else if (foodObject.GetComponent<IngredientObject>())
                {
                    // If it is a ingredient object
                    foodObject.SetActive(false);
                }

                ListOfPrepSlots[i].prepSlots[j].ResetSlot();
            }
        }
    }
}

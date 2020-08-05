using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DraggableSpawner : MonoBehaviour
{
    [Tooltip("AAAAAAAAAAAAAAAAAAAAAAAA")]
    public string ingredientTag;

    private bool inCollider = false;

    private bool Debounce = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnMouseDown()
    {
        if (Debounce)
            return;

        if (!inCollider)
            return;

        // so that the function only run once
        Debounce = true;

        GameObject newIngredient = ObjectPooler.Instance.FetchGO(ingredientTag);

        // Set it to dragging
        newIngredient.GetComponent<DraggableObjectController>().SetDrag(true);

    }

    private void OnMouseUp()
    {
        Debounce = false;
    }


    private void OnMouseOver()
    {
        inCollider = true;

        //Debug.Log("Mouse is over the object lol");
    }

    private void OnMouseExit()
    {
        inCollider = false;

        //Debug.Log("Not hovering over");
    }

}

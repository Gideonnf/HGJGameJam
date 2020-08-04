using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class DraggableObjectController : SingletonBase<DraggableObjectController>
{
    [SerializeField]
    //use this to automatically object snap back to position when mouse released
    private bool snapBackToStart = false;
    public bool isSetToObject = false;

    private Vector2 startPos;
    private bool isDragging = false;
    private bool inCollider = false;

    //use this to manually trigger snap back to position
    public bool errorPairFlag { get; set; }

    private void OnEnable()
    {
        errorPairFlag = false;
        this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        this.gameObject.GetComponent<Rigidbody2D>().useFullKinematicContacts = true;

        startPos = this.transform.position;
        
    }

    public void SetStartPos(Vector2 position)
    {
        startPos = position;
    }

    public void OnMouseDown()
    {
        isDragging = true;
    }

    public void OnMouseUp()
    {
        isDragging = false;

        if (!inCollider)
        {
            if (snapBackToStart)
            {
                this.transform.position = startPos;
            }
        }
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePos);
        }

        if (errorPairFlag)
        {
            errorPairFlag = false;

            this.transform.position = startPos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        inCollider = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == this.gameObject.name)
            return;

        if (!isDragging)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("DropLocation"))
            {
                this.transform.position = collision.transform.position;
                snapBackToStart = false;

                isSetToObject = true;
                this.transform.parent = collision.transform;

                inCollider = false;
            }
            else if (collision.gameObject.tag == "RubbishBin")
            {
                FoodManager.Instance.RemoveFromPrepSlot(gameObject);

                if (this.gameObject.GetComponent<FoodObject>() != null)
                {
                    this.gameObject.GetComponent<FoodObject>().ResetFood();
                }

                // unparent it
                gameObject.transform.parent = null;

                //setting all children (food items) false
                for (int i = 0; i < this.transform.childCount; ++i)
                {
                    this.transform.GetChild(i).gameObject.SetActive(false);
                }

                // set back to inactive for the object pooler
                this.gameObject.SetActive(false);
            }
            else
            {
                inCollider = false;

                if (snapBackToStart)
                {
                    this.transform.position = startPos;
                }
            }
        }
    }

    public void ResetPosition()
    {
        if (inCollider)
            return;

        // Set snap back to true
        snapBackToStart = true;

        // Set back to original position
        this.transform.position = startPos;
    }

    public bool GetDragging()
    {
        return isDragging;
    }

    public Vector2 GetStartPos()
    {
        return startPos;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        inCollider = false;
        isSetToObject = false;
        ResetPosition();
    }
}

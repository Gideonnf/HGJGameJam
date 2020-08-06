using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class DraggableObjectController : MonoBehaviour
{
    float m_ZOffset = 2.0f;

    [SerializeField]
    //use this to automatically object snap back to position when mouse released
    private bool snapBackToStart = false;
    public bool isSetToObject = false;

    private Vector3 startPos;
    public bool isDragging = false;

    public bool inCollider = false;

    //use this to manually trigger snap back to position
    public bool errorPairFlag { get; set; }

    private void OnEnable()
    {
        errorPairFlag = false;
        this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        this.gameObject.GetComponent<Rigidbody2D>().useFullKinematicContacts = true;

        startPos = this.transform.position;
        
    }

    public void SetStartPos(Vector3 position)
    {
        startPos = position;
    }

    public void SetDrag(bool DragBoolean)
    {
        isDragging = DragBoolean;
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

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePos);
            transform.position = new Vector3(transform.position.x, transform.position.y, startPos.z - m_ZOffset);
        }

        if (GetComponent<IngredientObject>())
        {
            if (!inCollider && !isDragging)
            {
                // If it can only be dragged
                if (GetComponent<IngredientObject>().OnlyDraggable)
                {
                    transform.position = startPos;

                    gameObject.SetActive(false);

                    return;
                }
            }
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
        inCollider = true;

        if (collision.gameObject.name == this.gameObject.name)
        {
            inCollider = false;
            return;
        }

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
                // set back to inactive for the object pooler
                this.gameObject.SetActive(false);
            }
            else if (collision.gameObject.tag == "Customer")
            {
                if (!collision.gameObject.GetComponent<Customer>().m_LeavingStall && this.gameObject.GetComponent<FoodObject>() != null)
                {
                    if (collision.gameObject.GetComponent<Customer>().CheckFood(this.gameObject.GetComponent<FoodObject>()))
                    {
                        // set back to inactive for the object pooler
                        this.gameObject.SetActive(false);
                    }
                }

                inCollider = false;
                ResetPosition();
            }
            else
            {
                inCollider = false;

                ResetPosition();
            }
        }
    }

    public void ResetPosition()
    {
        if (inCollider || isDragging)
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

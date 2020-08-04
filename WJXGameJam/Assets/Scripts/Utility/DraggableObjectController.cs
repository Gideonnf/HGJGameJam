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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == this.gameObject.name)
            return;

        inCollider = true;

        if (!isDragging)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("DropLocation"))
            {
                this.transform.position = collision.transform.position;
                snapBackToStart = false;

                isSetToObject = true;
                this.transform.parent = collision.transform;
            }
            else
            {
                if (snapBackToStart)
                {
                    this.transform.position = startPos;
                }
            }
        }
    }

    public void ResetPosition()
    {
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

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

    private Vector2 startPos;
    private bool isDragging = false;
    private bool inCollider = false;

    //use this to manually trigger snap back to position
    public bool errorPairFlag { get; set; }
    public Collision2D collisionInfo = null;

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
        inCollider = true;

        if (!isDragging)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("DropLocation"))
            {
                this.transform.position = collision.transform.position;
                snapBackToStart = false;

                collisionInfo = collision;
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

    private void OnCollisionExit2D(Collision2D collision)
    {
        inCollider = false;
        collisionInfo = null;
    }
}

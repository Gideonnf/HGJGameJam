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

    //use this to manually trigger snap back to position
    private bool errorPairFlag { get; set; }

    public void OnMouseDown()
    {
        isDragging = true;


    }

    public void OnMouseUp()
    {
        isDragging = false;

        if (snapBackToStart)
        {
            this.transform.position = startPos;
        }
    }

    private void OnEnable()
    {
        this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        this.gameObject.GetComponent<Rigidbody2D>().useFullKinematicContacts = true;

        startPos = this.transform.position;
        
    }

    public void SetStartPos(Vector2 position)
    {
        startPos = position;
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

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (!isDragging)
        {
            if (collision.gameObject.layer == 8)
            {
                this.transform.position = collision.transform.position;
                snapBackToStart = false;
            }
        }
    }
}

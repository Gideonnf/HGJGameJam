using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class DraggableObjectController : MonoBehaviour
{
    public bool snapBackToStart = false;

    private Vector2 startPos;
    private bool isDragging = false;

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

    private void Start()
    {
        this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        this.gameObject.GetComponent<Rigidbody2D>().useFullKinematicContacts = true;

        if (snapBackToStart)
        {
            startPos = this.transform.position;
        }
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePos);
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

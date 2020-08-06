using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;

public class SpriteButton : MonoBehaviour
{
    public int CareerProgress = -1;

    private bool thisSelected = false;

    private MasterConfig MC;

    private void Start()
    {
        MC = GameObject.Find("DoNotDestroy").GetComponent<MasterConfig>();
    }

    private void OnMouseDown()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.6f, 0.6f);

                thisSelected = true;
            }
        }
    }

    private void OnMouseUp()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);

        if (thisSelected)
        {
            thisSelected = false;

            if (!MC.master_isEndless)
                GameObject.Find("Background").GetComponent<MainMenuController>().SetCareerProgress(CareerProgress);
            else
                GameObject.Find("Background").GetComponent<MainMenuController>().SetEndlessMode(CareerProgress);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

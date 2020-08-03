using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScaler : MonoBehaviour
{
    public GameObject BackgroundObject;

    // Start is called before the first frame update
    void Start()
    {
        float height = Camera.main.orthographicSize * 2;
        float width = height * Screen.width / Screen.height; // basically height * screen aspect ratio

        Sprite s = BackgroundObject.GetComponent<SpriteRenderer>().sprite;
        float unitWidth = s.textureRect.width / s.pixelsPerUnit;
        float unitHeight = s.textureRect.height / s.pixelsPerUnit;

        transform.localScale = new Vector3(width / unitWidth, height / unitHeight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

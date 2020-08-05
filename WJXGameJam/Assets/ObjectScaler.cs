using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScaler : MonoBehaviour
{
    public GameObject BackgroundObject;

    private Vector2 resolution;

    // Start is called before the first frame update
    void Start()
    {
        if (BackgroundObject == null)
            return;

        float height = Camera.main.orthographicSize * 2;
        float width = height * Screen.width / Screen.height; // basically height * screen aspect ratio

        Sprite s = BackgroundObject.GetComponent<SpriteRenderer>().sprite;
        float unitWidth = s.textureRect.width / s.pixelsPerUnit;
        float unitHeight = s.textureRect.height / s.pixelsPerUnit;

        transform.localScale = new Vector3(width / unitWidth, height / unitHeight);

        resolution = new Vector2(Screen.width, Screen.height);
    }

    // Update is called once per frame
    void Update()
    {
        if (BackgroundObject == null)
            return;

        if (resolution.x != Screen.width || resolution.y != Screen.height)
        {
            // do your stuff
            float height = Camera.main.orthographicSize * 2;
            float width = height * Screen.width / Screen.height; // basically height * screen aspect ratio

            Sprite s = BackgroundObject.GetComponent<SpriteRenderer>().sprite;
            float unitWidth = s.textureRect.width / s.pixelsPerUnit;
            float unitHeight = s.textureRect.height / s.pixelsPerUnit;

            transform.localScale = new Vector3(width / unitWidth, height / unitHeight);


            resolution.x = Screen.width;
            resolution.y = Screen.height;
        }
    }

    public void SetUp()
    {
        float height = Camera.main.orthographicSize * 2;
        float width = height * Screen.width / Screen.height; // basically height * screen aspect ratio

        Sprite s = BackgroundObject.GetComponent<SpriteRenderer>().sprite;
        float unitWidth = s.textureRect.width / s.pixelsPerUnit;
        float unitHeight = s.textureRect.height / s.pixelsPerUnit;

        transform.localScale = new Vector3(width / unitWidth, height / unitHeight);

        resolution = new Vector2(Screen.width, Screen.height);
    }
}

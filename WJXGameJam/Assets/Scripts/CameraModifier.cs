using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModifier : MonoBehaviour
{
    public SpriteRenderer m_GameBackground;

    // Start is called before the first frame update
    void Awake()
    {
        float width = m_GameBackground.sprite.rect.width;

        Camera.main.orthographicSize = Screen.height / 100.0f * 0.5f;       //Camera.main.orthographicSize = (m_GameBackground.sprite.rect.height / (2.0f * 100.0f));
        Debug.Log(m_GameBackground.sprite.rect.height);
    }
}

using UnityEngine;

public class HighlightInteractable : MonoBehaviour
{
    public Color m_HighlightedColor = Color.white;

    SpriteRenderer m_SpriteRenderer;
    Color m_OriginalColor = Color.white;

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        if (m_SpriteRenderer != null)
            m_OriginalColor = m_SpriteRenderer.color;
    }

    private void OnMouseEnter()
    {
        m_SpriteRenderer.color = m_HighlightedColor;
    }

    private void OnMouseExit()
    {
        m_SpriteRenderer.color = m_OriginalColor;
    }

    private void OnDisable()
    {
        m_SpriteRenderer.color = m_OriginalColor;
    }
}

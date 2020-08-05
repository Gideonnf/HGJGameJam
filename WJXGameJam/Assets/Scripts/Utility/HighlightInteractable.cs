using Boo.Lang;
using UnityEngine;

public class HighlightInteractable : MonoBehaviour
{
    public Color m_HighlightedColor = Color.white;

    SpriteRenderer m_SpriteRenderer;

    List<SpriteRenderer> m_ChildSpriteRenderer = new List<SpriteRenderer>();

    Color m_OriginalColor = Color.white;

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        if (m_SpriteRenderer != null)
            m_OriginalColor = m_SpriteRenderer.color;

        foreach(Transform child in transform)
        {
            if (child.gameObject.GetComponent<SpriteRenderer>())
                m_ChildSpriteRenderer.Add(child.gameObject.GetComponent<SpriteRenderer>());
        }
    }

    private void OnMouseEnter()
    {
        m_SpriteRenderer.color = m_HighlightedColor;

        for(int i = 0; i < m_ChildSpriteRenderer.Count; ++i)
        {
            m_ChildSpriteRenderer[i].color = m_HighlightedColor;
        }
    }

    private void OnMouseExit()
    {
        m_SpriteRenderer.color = m_OriginalColor;

        for (int i = 0; i < m_ChildSpriteRenderer.Count; ++i)
        {
            m_ChildSpriteRenderer[i].color = m_OriginalColor;
        }

    }

    private void OnDisable()
    {
        m_SpriteRenderer.color = m_OriginalColor;

        for (int i = 0; i < m_ChildSpriteRenderer.Count; ++i)
        {
            m_ChildSpriteRenderer[i].color = m_OriginalColor;
        }
    }
}

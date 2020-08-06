using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Merlion : MonoBehaviour
{
    public Color m_HighlightedColor;

    [TextArea(1, 10)]
    public List<string> m_HeritageFacts = new List<string>();
    public TextMeshPro m_TextMeshPro;

    Animator m_Animator;
    SpriteRenderer m_SpriteRenderer;

    public void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Clicked()
    {
        if (m_TextMeshPro != null)
        {
            m_TextMeshPro.text = m_HeritageFacts[Random.Range(0, m_HeritageFacts.Count)];
            m_TextMeshPro.gameObject.SetActive(false);
        }

        if (m_Animator == null)
            return;

        m_Animator.SetTrigger("StartQuestion");
        SoundManager.Instance.Play("Roar");
        SoundManager.Instance.Play("WaterSpill");
    }

    public void OnMouseDown()
    {
        if ((m_Animator.GetCurrentAnimatorStateInfo(0).IsName("OpenMerlion")
            || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("CloseMerlion"))
            && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f
            )
            return;

        Clicked();
    }

    private void OnMouseEnter()
    {
        if ((m_Animator.GetCurrentAnimatorStateInfo(0).IsName("OpenMerlion")
        || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("CloseMerlion"))
        && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f
        )
            return;

        m_SpriteRenderer.color = m_HighlightedColor;
    }

    private void OnMouseExit()
    {
        if ((m_Animator.GetCurrentAnimatorStateInfo(0).IsName("OpenMerlion")
        || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("CloseMerlion"))
        && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f
        )
            return;

        m_SpriteRenderer.color = Color.white;
    }
}

using UnityEngine;

public class SoundAtStart : MonoBehaviour
{
    public string m_BackgroundSound = "";

    // Start is called before the first frame update
    void Start()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.Play(m_BackgroundSound);
    }
}

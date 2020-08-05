using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : SingletonBase<TransitionManager>
{
    [SerializeField]
    private Image transitionImage;

    public bool startTransition = false;
    public bool easeIn = true; //1 - ease in, 0 - ease out

    public float transitionSpeed = 0.3f;
    static float t = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        transitionImage.color = new Color(0, 0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (startTransition)
        {
            if (easeIn)
            {
                transitionImage.color = new Color(0, 0, 0, Mathf.Lerp(transitionImage.color.a, 0, t));
                t += transitionSpeed * Time.deltaTime;

                if (t >= 1)
                {
                    startTransition = false;
                    t = 0.0f;

                    if (!DataManager.Instance.isEndless)
                    {
                        DataManager.Instance.StartDay();
                    }
                }
            }
            else
            {
                transitionImage.color = new Color(0, 0, 0, Mathf.Lerp(transitionImage.color.a, 1, t));
                t += transitionSpeed * Time.deltaTime;

                if (t >= 1)
                {
                    startTransition = false;
                    t = 0.0f;
                }
            }
        }
    }
}

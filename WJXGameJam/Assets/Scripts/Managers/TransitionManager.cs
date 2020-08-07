using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : SingletonBase<TransitionManager>
{
    [SerializeField]
    private Image transitionImage;

    public bool startTransition { get; set; }
    public bool easeIn = true; //1 - ease in, 0 - ease out

    public float transitionSpeed = 0.1f;
    static float t = 0.0f;

    public TextMeshProUGUI DayText;
    public TextMeshProUGUI NumPlatesSold;

    bool playShuttleSound = false;
    string ShuttleNoise = "ShuttleNoise";

    // Start is called before the first frame update
    void Start()
    {
        NumPlatesSold.transform.parent.gameObject.SetActive(false);
        easeIn = true;

        //if (!DataManager.Instance.isEndless)
        //    transitionImage.color = new Color(0, 0, 0, 1);
        //else
        //    transitionImage.color = new Color(0, 0, 0, 0);
        if (!DataManager.Instance.isEndless)
            transitionImage.rectTransform.localPosition = new Vector3(0, 0, 0);
        //transitionImage.color = new Color(0, 0, 0, 1);
        else
            // start from the top
            transitionImage.rectTransform.localPosition = new Vector3(0, 1077, 0);


        if (GameObject.Find("DoNotDestory") == null)
        {
            startTransition = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (startTransition)
        {
            if (easeIn)
            {
                // Shutter going up

                if (playShuttleSound == false)
                {
                    SoundManager.Instance.Play(ShuttleNoise);
                    playShuttleSound = true;
                }

                NumPlatesSold.transform.parent.gameObject.SetActive(false);
                //transitionImage.color = new Color(0, 0, 0, Mathf.Lerp(transitionImage.color.a, 0, t));
                transitionImage.rectTransform.localPosition = new Vector3(0, Mathf.Lerp(transitionImage.rectTransform.localPosition.y, 1100, t));

                t += transitionSpeed * Time.deltaTime;

                if (t >= 0.2f)
                {
                    startTransition = false;
                    t = 0.0f;
                    easeIn = !easeIn;
                    // transitionImage.color = new Color(0, 0, 0, 0);

                    // reset
                    playShuttleSound = false;

                    if (!DataManager.Instance.isEndless)
                        DataManager.Instance.StartDay();
                    
                }
            }
            else
            {
                // Shutter has to fall down
                //transitionImage.rectTransform.position;

                if (playShuttleSound == false)
                {
                    SoundManager.Instance.Play(ShuttleNoise);
                    playShuttleSound = true;
                }


                //transitionImage.color = new Color(0, 0, 0, Mathf.Lerp(transitionImage.color.a, 1, t));

                transitionImage.rectTransform.localPosition = new Vector3(0, Mathf.Lerp(transitionImage.rectTransform.localPosition.y, 0, t));

                t += transitionSpeed * Time.deltaTime;

                if (t >= 0.2f)
                {
                    playShuttleSound = false;

                    if (!DataManager.Instance.isEndless)
                    {
                        startTransition = false;
                        t = 0.0f;
                        easeIn = !easeIn;
                        //transitionImage.color = new Color(0, 0, 0, 1);

                        NumPlatesSold.transform.parent.gameObject.SetActive(true);
                        NumPlatesSold.text = "You earned: $" + playerData.moneyPerDay[playerData.moneyPerDay.Count - 1];
                        DayText.fontSize = 45;
                        DayText.text = "Day " + DataManager.Instance.currentDay;
                    }
                    else
                    {
                        startTransition = false;
                        t = 0.0f;
                        easeIn = !easeIn;
                        //transitionImage.color = new Color(0, 0, 0, 1);

                        NumPlatesSold.transform.parent.gameObject.SetActive(true);
                        NumPlatesSold.text = "You earned: $" + playerData.moneyPerDay[playerData.moneyPerDay.Count - 1];
                        DayText.fontSize = 35;
                        DayText.text = "Customers served: " + playerData.customersPerDay[playerData.customersPerDay.Count - 1];
                    }
                }
            }
        }
    }
}

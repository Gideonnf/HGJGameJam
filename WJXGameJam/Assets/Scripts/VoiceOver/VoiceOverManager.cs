using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceOverManager : SingletonBase<VoiceOverManager>
{
    //get voice base on gender and language
    //pass in food data to see what type of food
    VoiceOverData m_VoiceOverData;

    public void Awake()
    {
        m_VoiceOverData = GetComponent<VoiceOverData>();
    }

    public void GetVoice(bool male = true, VoiceLanguages language = VoiceLanguages.ENGLISH )
    {

    }
}

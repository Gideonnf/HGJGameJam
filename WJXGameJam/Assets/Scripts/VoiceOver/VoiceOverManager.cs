using System;
using System.Collections.Generic;
using UnityEngine;

public class VoiceOverManager 
{
    List<string> m_OrderSoundQueue = new List<string>();
    AudioSource m_AudioSource;
    NPCVoiceOverData m_CurrVoice = null;

    public void Init(AudioSource audioSource)
    {
        m_AudioSource = audioSource;
    }
    
    public void PlayCustomerVoice(VoiceActions voiceAction)
    {
        if (m_CurrVoice == null)
            return;

        //pick the voice
        string soundName = PickSpeech(voiceAction);

        //play the sound
        Sound playingSound = Array.Find(VoiceOverData.Instance.m_VoicesList, sound => sound.m_Name == soundName);

        if (playingSound == null)
            return;

        if (m_AudioSource != null)
        {
            m_AudioSource.clip = playingSound.m_Clip;
            m_AudioSource.volume = playingSound.m_Volume;
            m_AudioSource.Play();
        }
    }

    public void PlayDefaultVoice(VoiceActions voiceAction, bool isMale)
    {
        string soundName = PickDefaultSpeech(voiceAction, isMale);

        //play the sound
        Sound playingSound = Array.Find(VoiceOverData.Instance.m_VoicesList, sound => sound.m_Name == soundName);

        if (playingSound == null)
            return;

        if (m_AudioSource != null)
        {
            m_AudioSource.clip = playingSound.m_Clip;
            m_AudioSource.volume = playingSound.m_Volume;
            m_AudioSource.Play();
        }
    }

    public string PickSpeech(VoiceActions voiceAction)
    {
        foreach (VoiceActionsSpeeches speech in m_CurrVoice.m_Speeches)
        {
            if (speech.m_Action == voiceAction)
                return speech.m_SoundName;
        }

        return "";
    }

    public string PickDefaultSpeech(VoiceActions voiceAction, bool isMale)
    {
        if (isMale)
        {
            foreach (VoiceActionsSpeeches speech in VoiceOverData.Instance.m_DefaultMaleVoiceLines.m_Speeches)
            {
                if (speech.m_Action == voiceAction)
                    return speech.m_SoundName;
            }
        }
        else
        {
            foreach (VoiceActionsSpeeches speech in VoiceOverData.Instance.m_DefaultFemaleVoiceLines.m_Speeches)
            {
                if (speech.m_Action == voiceAction)
                    return speech.m_SoundName;
            }
        }

        return "";
    }

    public void PickVoice(bool isMale = true, VoiceLanguages language = VoiceLanguages.ENGLISH)
    {
        List<NPCVoiceOverData> m_AvailableVoiceTypes = new List<NPCVoiceOverData>();

        //find the ones with the proper languages and gender
        if (isMale)
        {
            foreach (NPCVoiceOverData voices in VoiceOverData.Instance.m_MaleVoiceLines)
            {
                if (voices.m_Language == language)
                {
                    m_AvailableVoiceTypes.Add(voices);
                }
            }
        }
        else
        {
            foreach (NPCVoiceOverData voices in VoiceOverData.Instance.m_FemaleVoiceLines)
            {
                if (voices.m_Language == language)
                {
                    m_AvailableVoiceTypes.Add(voices);
                }
            }
        }

        if (m_AvailableVoiceTypes.Count > 0)
            m_CurrVoice = m_AvailableVoiceTypes[UnityEngine.Random.Range(0, m_AvailableVoiceTypes.Count)];
    }
}

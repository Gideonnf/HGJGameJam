using System;
using System.Collections.Generic;
using UnityEngine;

public class VoiceOverManager 
{
    List<string> m_OrderSoundQueue = new List<string>();
    AudioSource m_AudioSource;

    public void Init(AudioSource audioSource)
    {
        m_AudioSource = audioSource;
    }
    
    public void PlayCustomerVoice(bool isMale = true, VoiceLanguages language = VoiceLanguages.ENGLISH)
    {
        //pick the voice
        string soundName = PickVoice(isMale, language);

        //play the sound
        Sound playingSound = Array.Find(VoiceOverData.Instance.m_VoicesList, sound => sound.m_Name == soundName);

        if (playingSound == null)
            return;

        if (m_AudioSource != null)
        {
            m_AudioSource.clip = playingSound.m_Clip;
            m_AudioSource.Play();
        }
    }

    public string PickVoice(bool isMale = true, VoiceLanguages language = VoiceLanguages.ENGLISH)
    {
        List<string> m_VoiceLines = new List<string>();

        //find the ones with the proper languages
        if (isMale)
        {
            foreach (NPCVoiceOverData voices in VoiceOverData.Instance.m_MaleVoiceLines)
            {
                if (voices.m_Language == language)
                {
                    m_VoiceLines.Add(voices.m_SoundName);
                }
            }
        }
        else
        {
            foreach (NPCVoiceOverData voices in VoiceOverData.Instance.m_FemaleVoiceLines)
            {
                if (voices.m_Language == language)
                {
                    m_VoiceLines.Add(voices.m_SoundName);
                }
            }
        }

        if (m_VoiceLines.Count > 0)
            return m_VoiceLines[UnityEngine.Random.Range(0, m_VoiceLines.Count)];

        return null;
    }
}

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VoiceOverData : SingletonBase<VoiceOverData>
{
    public FoodStage m_FoodStage;

    [Header("FemaleVoiceLines")]
    public List<NPCVoiceOverData> m_FemaleVoiceLines = new List<NPCVoiceOverData>();
    [Header("MaleVoiceLines")]
    public List<NPCVoiceOverData> m_MaleVoiceLines = new List<NPCVoiceOverData>();

    [Header("Voices")]
    public Sound[] m_VoicesList;
}

[System.Serializable]
public class NPCVoiceOverData
{
    public VoiceLanguages m_Language = VoiceLanguages.ENGLISH;
    public string m_SoundName;
}

public enum VoiceLanguages
{
    ENGLISH,
    MALAY,
    CHINESE,
}


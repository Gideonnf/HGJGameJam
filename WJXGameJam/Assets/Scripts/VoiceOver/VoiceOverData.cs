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
    public List<VoiceActionsSpeeches> m_Speeches = new List<VoiceActionsSpeeches>();
}

[System.Serializable]
public class VoiceActionsSpeeches
{
    public VoiceActions m_Action = VoiceActions.GREETING;
    public string m_SoundName = "";
}

public enum VoiceLanguages
{
    ENGLISH,
    MALAY,
    CHINESE,
}

public enum VoiceActions
{
    GREETING, //when just enter in
    GETTING_IMPATIENT,
    ANGRY,
    ANGRY_LEAVE, //dint get order or 
    SATISFIED, //get order properly, thank the guy
}

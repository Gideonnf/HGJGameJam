using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VoiceOverData : SingletonBase<VoiceOverData>
{
    public FoodStage m_FoodStage;

    public List<NPCVoiceOverData> m_FemaleVoiceLines = new List<NPCVoiceOverData>();
    public List<NPCVoiceOverData> m_MaleVoiceLines = new List<NPCVoiceOverData>();
}

[System.Serializable]
public class NPCVoiceOverData
{
    public VoiceLanguages m_Language = VoiceLanguages.ENGLISH;
    public VoiceFoodOrderData m_FoodOrderVoiceOverData;
}

[System.Serializable]
public class VoiceFoodOrderData
{
    public MainIngredient m_MainIngredient = MainIngredient.Rice;

    //if only one ingredient, the texts he gonna say
    [Tooltip("Text for main sub ingredient")]
    public List<VoiceSubIngredientOrderData> m_MainSubIngredintVoiceLines = new List<VoiceSubIngredientOrderData>();

    [Tooltip("Possible additional lines based on the additional sub ingredients")]
    public List<VoiceSubIngredientOrderData> m_AdditionalSubIngredintVoiceLines = new List<VoiceSubIngredientOrderData>();
}

public class VoiceSubIngredientOrderData
{
    public SubIngredient m_SubIngredient = SubIngredient.Egg;
    public string m_SoundClipName;
}

public enum VoiceLanguages
{
    ENGLISH,
    MALAY,
    CHINESE,
}


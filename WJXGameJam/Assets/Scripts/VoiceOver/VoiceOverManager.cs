using System.Collections.Generic;
using UnityEngine;

public class VoiceOverManager 
{
    List<string> m_OrderSoundQueue = new List<string>();

    public void StartOrder(FoodData foodData, bool isMale = true, VoiceLanguages language = VoiceLanguages.ENGLISH)
    {
        m_OrderSoundQueue.Clear();

        VoiceFoodOrderData voiceChosen = PickVoice(foodData.mainIngredient, isMale, language);

        //add to the queue the order
        int numberOfIngredients = foodData.ListOfSubIngredients.Count;
        for (int i = 0; i < numberOfIngredients; ++i)
        {
            bool nextIngredient = false;

            //check main subingredient first
            foreach (VoiceSubIngredientOrderData mainSubIngredientVoice in voiceChosen.m_MainSubIngredintVoiceLines)
            {
                if (mainSubIngredientVoice.m_SubIngredient == foodData.ListOfSubIngredients[i])
                {
                    m_OrderSoundQueue.Add(mainSubIngredientVoice.m_SoundClipName);
                    nextIngredient = true;
                    break;
                }
            }

            if (nextIngredient) //check next ingredient
                continue;

            //add to the queue the other orders
            foreach (VoiceSubIngredientOrderData additionalSubIngredientVoice in voiceChosen.m_AdditionalSubIngredintVoiceLines)
            {
                if (additionalSubIngredientVoice.m_SubIngredient == foodData.ListOfSubIngredients[i])
                {
                    m_OrderSoundQueue.Add(additionalSubIngredientVoice.m_SoundClipName);
                    break;
                }
            }
        }
    }

    public VoiceFoodOrderData PickVoice(MainIngredient mainIngredient, bool isMale = true, VoiceLanguages language = VoiceLanguages.ENGLISH)
    {
        List<VoiceFoodOrderData> m_VoiceLines = new List<VoiceFoodOrderData>();

        //find the ones with the proper languages
        if (isMale)
        {
            foreach (NPCVoiceOverData voices in VoiceOverData.Instance.m_MaleVoiceLines)
            {
                if (voices.m_Language == language)
                {
                    m_VoiceLines.Add(voices.m_FoodOrderVoiceOverData);
                }
            }
        }
        else
        {
            foreach (NPCVoiceOverData voices in VoiceOverData.Instance.m_FemaleVoiceLines)
            {
                if (voices.m_Language == language)
                {
                    m_VoiceLines.Add(voices.m_FoodOrderVoiceOverData);
                }
            }
        }

        //get the correct main ingredient food order voice line
        List<VoiceFoodOrderData> m_tempVoiceLineStorage = new List<VoiceFoodOrderData>();
        foreach (VoiceFoodOrderData foodOrderVoice in m_VoiceLines)
        {
            if (foodOrderVoice.m_MainIngredient == mainIngredient)
            {
                m_tempVoiceLineStorage.Add(foodOrderVoice);
            }
        }

        if (m_tempVoiceLineStorage.Count > 0)
            return m_tempVoiceLineStorage[Random.Range(0, m_tempVoiceLineStorage.Count)];

        return null;
    }
}

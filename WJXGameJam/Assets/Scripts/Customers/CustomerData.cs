using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomerData 
{
    [Header("NPC expressions")]
    public List<CustomerMood> m_CustomerMoodDataList = new List<CustomerMood>();

    [Header("NPC appearances")]
    public List<CustomerInfo> m_CustomerInfo = new List<CustomerInfo>();

    public Sprite GetCustomerFacialSprite(int index)
    {
        if (index >= m_CustomerMoodDataList.Count)
            return null;

        return m_CustomerMoodDataList[index].m_FacialExpressionSprite;
    }
}

[System.Serializable]
public class CustomerInfo
{
    [Tooltip("avilable languages he can speak")]
    public List<VoiceLanguages> m_AvailableLaungages = new List<VoiceLanguages>();
    public bool m_IsMale = false;
    public Sprite m_NPCSprite;
}

[System.Serializable]
public class CustomerMood
{
    public CustomerExpressions m_Expressions = CustomerExpressions.HAPPY;
    [Tooltip("The min percentage of the total patience time for this expression")]
    [Range(0.0f, 1.0f)]
    public float m_MinPercentageForExpression = 0.0f;

    public Sprite m_FacialExpressionSprite;
}

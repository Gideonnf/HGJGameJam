using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterConfig : SingletonBase<MasterConfig>
{
    public bool master_isEndless;
    public FoodStage master_foodStage = FoodStage.Chinatown;

    public int master_currentDay = 0;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AgentRewardCalculatorBase : MonoBehaviour
{
    public abstract List<float> GetReward();

    

}

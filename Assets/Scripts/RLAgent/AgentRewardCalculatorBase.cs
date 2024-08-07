using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AgentRewardCalculatorBase : MonoBehaviour
{
    public abstract List<float> CalculateReward();
    protected float step_reward ;

    public void SetReward(float reward)
    {
        this.step_reward = reward;
    }

    public void AddReward(float reward)
    {
        this.step_reward += reward;
    }

    

}

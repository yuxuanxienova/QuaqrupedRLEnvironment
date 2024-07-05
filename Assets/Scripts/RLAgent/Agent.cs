using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [Header("Settings")][Space(10)]
    public AgentObserverBase agentObserver;
    public AgentControllerBase agentController;
    public AgentRewardCalculatorBase agentRewardCalculator;

    private bool trancated_flag=false;

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Reset()
    {

        agentController.Reset();
        trancated_flag=true;
    }
    public void SetTrancatedFlag(bool flag)
    {
        trancated_flag=flag;
    }
    public void SetAction(List<float> action_list)
    {
        agentController.SetAction(action_list);
    }

    public float[] GetAction()
    {
        List<float> action_list = agentController.GetAction();
        if(action_list != null)
        {
            return action_list.ToArray();
        }
        else
        {
            return new float[0];
        }
    }

    public float[] GetObservation()
    {
        List<float> observation_list = agentObserver.GetObservations();
        if(observation_list != null)
        {
            return observation_list.ToArray();
        }
        else
        {
            return new float[0];
        }
    }
    public float[] CalculateReward()
    {
        List<float> reward_list = agentRewardCalculator.GetReward();
        if(reward_list != null)
        {
            return reward_list.ToArray();
        }
        else
        {
            return new float[0];
        }
    }
}

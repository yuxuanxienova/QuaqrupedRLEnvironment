using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadrupedAgentRewardCalculator : AgentRewardCalculatorBase
{
    private Agent agent;
    private QuadrupedAgentController agentController;
    public override List<float> CalculateReward()
    {
        if (agentController.BodyTouchingGround())
        {
            SetStepReward(-1f);
            agent.Reset();
        }
        else
        {
            SetStepReward(0.1f);
        }

        episode_reward += step_reward;
        return new List<float>{step_reward};

    }

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<Agent>();
        agentController = gameObject.GetComponent<QuadrupedAgentController>();
        episode_reward = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


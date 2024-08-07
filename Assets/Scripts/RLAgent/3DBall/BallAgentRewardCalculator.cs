using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAgentRewardCalculator : AgentRewardCalculatorBase
{
    public GameObject ball;
    private Agent agent;
    public override List<float> CalculateReward()
    {
        if ((ball.transform.position.y - gameObject.transform.position.y) < -2f ||
            Mathf.Abs(ball.transform.position.x - gameObject.transform.position.x) > 3f ||
            Mathf.Abs(ball.transform.position.z - gameObject.transform.position.z) > 3f)
        {
            SetReward(-1f);
            agent.Reset();
        }
        else
        {
            SetReward(0.1f);
        }
        return new List<float>{step_reward};

    }

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<Agent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

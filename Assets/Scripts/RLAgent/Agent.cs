using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class Agent : MonoBehaviour
{
    [Header("Settings")][Space(10)]
    public AgentObserverBase agentObserver;
    public AgentControllerBase agentController;
    public AgentRewardCalculatorBase agentRewardCalculator;

    public ServiceClientSampleAction serviceClientSampleAction;
    public PublishTransition publishTransition;

    private bool trancated_flag=false;

    // private float EpisodicReturn = 0;

    // private float timeElapsed_rewardCalculation = 0;
    // public float rewardCalculationInterval = 0.01f ;

    private float timeElapsed_actionUpdate = 0;
    public float actionUpdateInterval = 0.2f;

    private float timeElapsed_transitionPublishUpdate = 0;
    public float transitionPublishUpdateInterval = 0.1f;

    private float[] state_stored;
    private float[] action_stored;

    private int episodeCount = 0;

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        // UpdateEpisodicReturn();
        UpdateAction();
        UpdateTransitionPublish();

    }
    public void Reset()
    {
        episodeCount += 1;
        agentController.Reset();
        // Debug.Log("[INFO][Agent]" + "Episode="+ episodeCount + "EpisodicReturn="+EpisodicReturn);
        // ResetEpisodicReturn();
        trancated_flag=true;
    }
    private void UpdateAction()
    {
        timeElapsed_actionUpdate += Time.deltaTime;
        if(timeElapsed_actionUpdate > actionUpdateInterval)
        {
            //Start a coroutine 
            StartCoroutine(CallAsyncUpdateAction());
            timeElapsed_actionUpdate=0;
        }
    }
    private IEnumerator CallAsyncUpdateAction()
    {
        float[] obs_arr = GetObservation();
        var task = serviceClientSampleAction.SampleActionFromObservationAsync(obs_arr);
        // Wait for the task to complete
        yield return new WaitUntil(() => task.IsCompleted);
        // Handle the result
        if (task.IsFaulted)
        {
            Debug.LogError("Error in async operation: " + task.Exception);
        }
        else if (task.IsCanceled)
        {
            Debug.LogWarning("Async operation was canceled");
        }
        else
        {
            float[] result = task.Result;
            List<float> floatList = new List<float>(result);
            SetExecuteAction(floatList);
        }

    } 
    private void UpdateTransitionPublish()
    {
        timeElapsed_transitionPublishUpdate += Time.deltaTime;
        if(timeElapsed_transitionPublishUpdate > transitionPublishUpdateInterval)
        {
            float[] state_tminus1 = state_stored;
            float[] action_tminus1 = action_stored;
            float[] reward_tminus1 = CalculateReward();
            float[] state_t = GetObservation();

            state_stored = GetObservation();
            action_stored = GetAction();

            publishTransition.CallPublishTransition(state_tminus1,action_tminus1,reward_tminus1,state_t,trancated_flag);
            trancated_flag=false;
            timeElapsed_transitionPublishUpdate=0;
        }
    }

    public void SetTrancatedFlag(bool flag)
    {
        trancated_flag=flag;
    }
    public bool GetTrancaredFlag()
    {
        return trancated_flag;
    }
    public void SetExecuteAction(List<float> action_list)
    {
        agentController.SetAction(action_list);
        agentController.ExecuteAction();
    }
    public async Task<float[]> SampleActionAsync(float[] obs_arr)
    {
        float[] action_arr = await serviceClientSampleAction.SampleActionFromObservationAsync(obs_arr);
        return action_arr;
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
        List<float> reward_list = agentRewardCalculator.CalculateReward();
        if(reward_list != null)
        {
            return reward_list.ToArray();
        }
        else
        {
            return new float[0];
        }
    }

    // public void UpdateEpisodicReturn()
    // {
    //     timeElapsed_rewardCalculation += Time.deltaTime;
    //     if(timeElapsed_rewardCalculation > rewardCalculationInterval)
    //     {
    //         EpisodicReturn += CalculateReward()[0];
    //         timeElapsed_rewardCalculation=0;
    //     }
    // }
    // public void ResetEpisodicReturn()
    // {
    //     EpisodicReturn = 0;
    // }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class Agent : MonoBehaviour
{    
    private static int nextId = 0; // Static counter to track the next available ID

    [Header("Settings")][Space(10)]
    public AgentObserverBase agentObserver;
    public AgentControllerBase agentController;
    public AgentRewardCalculatorBase agentRewardCalculator;

    // public ServiceClientSampleAction serviceClientSampleAction;
    public PublishEventSampleAction publishEventSampleAction;
    public PublishTransition publishTransition;
    public PublishEpisodicReward publishEpisodicReward;
    private bool trancated_flag=false;

    private float timeElapsed_actionUpdate = 0;
    public float actionUpdateInterval = 0.2f;

    private float timeElapsed_transitionPublishUpdate = 0;
    public float transitionPublishUpdateInterval = 0.1f;

    private float[] state_stored;
    private float[] action_stored;

    private int episodeCount = 0;

    public int id = 0;

    private int num_update_action=0;

    void Start()
    {
        // Assign a unique ID to this agent
        id = nextId;
        nextId++; // Increment the static counter for the next agent

        // Add this agent to the AgentManager dictionary
        AgentManager.Instance.AddAgentToDict(id, this);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAction();
        UpdateTransitionPublish();

    }
    public void Reset()
    {
        episodeCount += 1;
        publishEpisodicReward.CallPublishEpisodicReward(agentRewardCalculator.GetEpisodeReward());
        Debug.Log($"[INFO][Agent][agent_id={id}][episodeCount={episodeCount}][episodeReward={agentRewardCalculator.GetEpisodeReward()}])");
        Debug.Log($"[INFO][Agent][agent_id={id}][episodeCount={episodeCount}]Resetting agent");
        agentController.Reset();
        agentRewardCalculator.ResetEpisodeReward();
        trancated_flag=true;
    }
    private void UpdateAction()
    {
        timeElapsed_actionUpdate += Time.deltaTime;
        if(timeElapsed_actionUpdate > actionUpdateInterval)
        {
            num_update_action+=1;
            
            //Start a coroutine 
            // StartCoroutine(CallAsyncUpdateAction());
            float[] obs_arr = GetObservation();
            // UnityEngine.Debug.Log($"[INFO][Agent][agent_id={id}][num_update_action={num_update_action}]Observation:"+ExtensionMethods.FloatArrayToString(obs_arr));

            // serviceClientSampleAction.SampleActionFromObservationServiceRequest(obs_arr, OnAgentSampleActionResponse);
            publishEventSampleAction.CallPublishEventSampleAction(obs_arr,id_agent:id);
            timeElapsed_actionUpdate=0;

        }
    }
    void OnAgentSampleActionResponse(float[] action)
    {
        // Handle the response specific to this agent
        // Debug.Log($"[INFO][Agent][agent_id={id}][num_update_action={num_update_action}]Received action: " + string.Join(",", action));
        List<float> floatList = new List<float>(action);
        SetExecuteAction(floatList);
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
        agentObserver.CheckObservationListDim();
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
}

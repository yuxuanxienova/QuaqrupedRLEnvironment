using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [Header("Settings")][Space(10)]
    public AgentObserverBase agentObserver;
    public AgentControllerBase agentController;

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetAction(List<float> action_list)
    {
        agentController.SetAction(action_list);
    }

    public List<float> GetObservation()
    {
        return agentObserver.GetObservations();
    }
}

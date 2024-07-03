using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [Header("Settings")][Space(10)]
    public AgentObserver agentObserver;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public List<float> GetObservation()
    {
        return agentObserver.GetObservations();
    }
}

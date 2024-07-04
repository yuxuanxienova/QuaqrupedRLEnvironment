using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadrupedAgentObserver : AgentObserverBase
{
    [Header("Body Parts")][Space(10)] 
    public Transform body;
    public Transform RH_HIP;
    public Transform RH_THIGH;
    public Transform RH_SHANK;
    public Transform LH_HIP;
    public Transform LH_THIGH;
    public Transform LH_SHANK;
    public Transform RF_HIP;
    public Transform RF_THIGH;
    public Transform RF_SHANK;
    public Transform LF_HIP;
    public Transform LF_THIGH;
    public Transform LF_SHANK;

    private ArticulationBody articulationBody_Body;

    private ArticulationBody articulationBody_RH_HIP;
    private ArticulationBody articulationBody_RH_THIGH;
    private ArticulationBody articulationBody_RH_SHANK;

    private ArticulationBody articulationBody_LH_HIP;
    private ArticulationBody articulationBody_LH_THIGH;
    private ArticulationBody articulationBody_LH_SHANK;

    private ArticulationBody articulationBody_RF_HIP;
    private ArticulationBody articulationBody_RF_THIGH;
    private ArticulationBody articulationBody_RF_SHANK;

    private ArticulationBody articulationBody_LF_HIP;
    private ArticulationBody articulationBody_LF_THIGH;
    private ArticulationBody articulationBody_LF_SHANK;

    void Start()
    {
        articulationBody_Body = body.GetComponent<ArticulationBody>();

        articulationBody_RH_HIP = RH_HIP.GetComponent<ArticulationBody>();
        articulationBody_RH_THIGH = RH_THIGH.GetComponent<ArticulationBody>();
        articulationBody_RH_SHANK = RH_SHANK.GetComponent<ArticulationBody>();

        articulationBody_LH_HIP = LH_HIP.GetComponent<ArticulationBody>();
        articulationBody_LH_THIGH = LH_THIGH.GetComponent<ArticulationBody>();
        articulationBody_LH_SHANK = LH_SHANK.GetComponent<ArticulationBody>();

        articulationBody_RF_HIP = RF_HIP.GetComponent<ArticulationBody>();
        articulationBody_RF_THIGH = RF_THIGH.GetComponent<ArticulationBody>();
        articulationBody_RF_SHANK = RF_SHANK.GetComponent<ArticulationBody>();    

        articulationBody_LF_HIP = LF_HIP.GetComponent<ArticulationBody>();
        articulationBody_LF_THIGH = LF_THIGH.GetComponent<ArticulationBody>();
        articulationBody_LF_SHANK = LF_SHANK.GetComponent<ArticulationBody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override List<float> GetObservations()
    {
        observation_list = new List<float>();
        AddToObservationList(articulationBody_RH_HIP.jointPosition[0],name:"[articulationBody_RH_HIP.jointPosition[0]]");
        AddToObservationList(articulationBody_RH_THIGH.jointPosition[0],name:"[articulationBody_RH_THIGH.jointPosition[0]]");
        AddToObservationList(articulationBody_RH_SHANK.jointPosition[0],name:"[articulationBody_RH_SHANK.jointPosition[0]]");
        return observation_list;
    }

}

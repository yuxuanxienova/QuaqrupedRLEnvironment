using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadrupedAgentObserver : AgentObserverBase
{
 
 

    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override List<float> GetObservations()
    {
        observation_list = new List<float>();
        // AddToObservationList(articulationBody_RH_HIP.jointPosition[0],name:"[articulationBody_RH_HIP.jointPosition[0]]");

        return observation_list;
    }

}

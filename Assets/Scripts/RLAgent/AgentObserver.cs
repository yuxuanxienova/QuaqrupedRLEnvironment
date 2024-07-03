using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentObserver : MonoBehaviour
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

    private List<float> observation_list;
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

    public List<float> GetObservations()
    {
        observation_list = new List<float>();
        AddObservation(articulationBody_RH_HIP.jointPosition[0],name:"[articulationBody_RH_HIP.jointPosition[0]]");
        AddObservation(articulationBody_RH_THIGH.jointPosition[0],name:"[articulationBody_RH_THIGH.jointPosition[0]]");
        AddObservation(articulationBody_RH_SHANK.jointPosition[0],name:"[articulationBody_RH_SHANK.jointPosition[0]]");
        return observation_list;
    }


    //Utilities
    void AddFloatObs(float obs, string name)
    {
        if (float.IsNaN(obs))
        {
            Debug.LogError("[ERROR][AgentObserver][AddFloatObs]" + name + " is NaN !!!");
        }
        if (float.IsInfinity(obs))
        {
            Debug.LogError("[ERROR][AgentObserver][AddFloatObs]" + name + " is Infinity !!!");
        }

        observation_list.Add(obs);
        //Print Info
        Debug.Log("[INFO][AgentObserver][AddFloatObs]"+ name + " is " + obs);
    }

    // Compatibility methods with Agent observation. These should be removed eventually.

    /// <summary>
    /// Adds a float observation to the vector observations of the agent.
    /// </summary>
    /// <param name="observation">Observation.</param>
    public void AddObservation(float observation, string name)
    {
        AddFloatObs(observation, name);
    }

    /// <summary>
    /// Adds an integer observation to the vector observations of the agent.
    /// </summary>
    /// <param name="observation">Observation.</param>
    public void AddObservation(int observation, string name)
    {
        AddFloatObs(observation, name);
    }

    /// <summary>
    /// Adds an Vector3 observation to the vector observations of the agent.
    /// </summary>
    /// <param name="observation">Observation.</param>
    public void AddObservation(Vector3 observation, string name)
    {
        AddFloatObs(observation.x, name + "[x]");
        AddFloatObs(observation.y, name + "[y]");
        AddFloatObs(observation.z, name + "[z]");
    }

    /// <summary>
    /// Adds an Vector2 observation to the vector observations of the agent.
    /// </summary>
    /// <param name="observation">Observation.</param>
    public void AddObservation(Vector2 observation, string name)
    {
        AddFloatObs(observation.x, name + "[x]");
        AddFloatObs(observation.y, name + "[y]");
    }

    /// <summary>
    /// Adds a list or array of float observations to the vector observations of the agent.
    /// </summary>
    /// <param name="observation">Observation.</param>
    public void AddObservation(IList<float> observation, string name)
    {
        for (var i = 0; i < observation.Count; i++)
        {
            AddFloatObs(observation[i], name +"["+i+"]");
        }
    }

    /// <summary>
    /// Adds a quaternion observation to the vector observations of the agent.
    /// </summary>
    /// <param name="observation">Observation.</param>
    public void AddObservation(Quaternion observation, string name)
    {
        AddFloatObs(observation.x, name + "[x]");
        AddFloatObs(observation.y, name + "[y]");
        AddFloatObs(observation.z, name + "[z]");
        AddFloatObs(observation.w, name + "[w]");
    }

    /// <summary>
    /// Adds a boolean observation to the vector observation of the agent.
    /// </summary>
    /// <param name="observation">Observation.</param>
    public void AddObservation(bool observation)
    {
        AddFloatObs(observation ? 1f : 0f, name);
    }
}

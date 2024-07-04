using System.Collections.Generic;
using UnityEngine;

public abstract class AgentObserverBase : MonoBehaviour, IAgentObserver
{
    public abstract List<float> GetObservations();
    protected List<float> observation_list;

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
    public void AddToObservationList(float observation, string name)
    {
        AddFloatObs(observation, name);
    }

    /// <summary>
    /// Adds an integer observation to the vector observations of the agent.
    /// </summary>
    /// <param name="observation">Observation.</param>
    public void AddToObservationList(int observation, string name)
    {
        AddFloatObs(observation, name);
    }

    /// <summary>
    /// Adds an Vector3 observation to the vector observations of the agent.
    /// </summary>
    /// <param name="observation">Observation.</param>
    public void AddToObservationList(Vector3 observation, string name)
    {
        AddFloatObs(observation.x, name + "[x]");
        AddFloatObs(observation.y, name + "[y]");
        AddFloatObs(observation.z, name + "[z]");
    }

    /// <summary>
    /// Adds an Vector2 observation to the vector observations of the agent.
    /// </summary>
    /// <param name="observation">Observation.</param>
    public void AddToObservationList(Vector2 observation, string name)
    {
        AddFloatObs(observation.x, name + "[x]");
        AddFloatObs(observation.y, name + "[y]");
    }

    /// <summary>
    /// Adds a list or array of float observations to the vector observations of the agent.
    /// </summary>
    /// <param name="observation">Observation.</param>
    public void AddToObservationList(IList<float> observation, string name)
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
    public void AddToObservationList(Quaternion observation, string name)
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
    public void AddToObservationList(bool observation)
    {
        AddFloatObs(observation ? 1f : 0f, name);
    }
}

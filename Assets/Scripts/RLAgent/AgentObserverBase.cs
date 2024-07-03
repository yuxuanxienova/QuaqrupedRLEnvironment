using System.Collections.Generic;
using UnityEngine;

public abstract class AgentObserverBase : MonoBehaviour, IAgentObserver
{
    public abstract List<float> GetObservations();
}

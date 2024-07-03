using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAgentObserver 
{
    public List<float> GetObservations();

}

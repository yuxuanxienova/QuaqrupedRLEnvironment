using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AgentControllerBase : MonoBehaviour,IAgentController
{
    public abstract void SetAction(List<float> action_list);

    protected List<float> action_list;

}

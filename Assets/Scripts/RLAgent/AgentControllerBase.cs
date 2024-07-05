using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AgentControllerBase : MonoBehaviour
{
    public abstract void SetAction(List<float> action_list);
    public List<float> GetAction()
    {
        return action_list;
    }

    protected List<float> action_list;

}

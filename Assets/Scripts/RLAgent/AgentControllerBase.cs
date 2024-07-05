using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AgentControllerBase : MonoBehaviour
{
    public abstract void SetAction(List<float> action_list);
    public abstract void Reset();
    public List<float> GetAction()
    {
        return action_list;
    }

    protected List<float> action_list;

    public static float MapValue(float value, float fromLow, float fromHigh, float toLow, float toHigh)
    {
        // Normalize the value to the range [0, 1]
        float normalizedValue = (value - fromLow) / (fromHigh - fromLow);
        
        // Map the normalized value to the target range
        float mappedValue = toLow + normalizedValue * (toHigh - toLow);
        
        return mappedValue;
    }

}

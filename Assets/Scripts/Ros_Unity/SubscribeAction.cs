using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;
public class SubscribeAction : MonoBehaviour
{
    public Agent agent;
    public string topic_name = "/trainner_node/event/set_action";
    // Start is called before the first frame update
    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<Float32MultiArrayMsg>(topic_name,onCallSubscribeSetAction);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void onCallSubscribeSetAction(Float32MultiArrayMsg multiArrayMsg)
    {
        // Extract float data from Float32MultiArrayMsg
        List<float> floatList = new List<float>();
        foreach (var value in multiArrayMsg.data)
        {
            floatList.Add(value);
        }
        
        // Pass the extracted float data to agent.SetAction()
        agent.SetExecuteAction(floatList);

    }
}

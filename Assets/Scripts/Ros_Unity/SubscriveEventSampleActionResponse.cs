using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;
using RosMessageTypes.RLTrainnerPkg;
public class SubscriveEventSampleActionResponse : MonoBehaviour
{
    public string topic_name = "/unity/RL_Agent/event_sample_action_response";
    // Start is called before the first frame update
    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<Float32IDMsgMsg>(topic_name,onCallSubscribeEventSampleActionResponse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onCallSubscribeEventSampleActionResponse(Float32IDMsgMsg msg)
    {
        // Extract float data from Float32IDMsgMsg
        List<float> floatList = new List<float>();
        int id = msg.id;
        foreach (var value in msg.data)
        {
            floatList.Add(value);
        }
        
        // Pass the extracted float data to agent.SetAction()
        Agent agent = AgentManager.Instance.idToAgentDict[id];
        agent.SetExecuteAction(floatList);
    }
}

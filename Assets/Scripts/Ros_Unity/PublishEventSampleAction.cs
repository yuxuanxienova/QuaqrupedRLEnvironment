using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.RLTrainnerPkg;
using RosMessageTypes.Std;
public class PublishEventSampleAction : MonoBehaviour
{
    ROSConnection rosConnection;
    public string topicName="/unity/RL_Agent/event_sample_action";
    // Start is called before the first frame update
    void Start()
    {
        rosConnection = ROSConnection.GetOrCreateInstance();
        rosConnection.RegisterPublisher<Float32IDMsgMsg>(topicName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallPublishEventSampleAction(float[] obs_array, int id_agent)
    {
        Float32IDMsgMsg message = new Float32IDMsgMsg
        {
            data = obs_array,
            id = id_agent
        };

        rosConnection.Publish(topicName,message);
    }
}

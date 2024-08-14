using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;
public class PublishEpisodicReward : MonoBehaviour
{

    ROSConnection rosConnection;
    public string topicName="/unity/RL_Agent/episodic_reward";
    // Start is called before the first frame update
    void Start()
    {
        rosConnection = ROSConnection.GetOrCreateInstance();
        rosConnection.RegisterPublisher<Float32Msg>(topicName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallPublishEpisodicReward(float reward)
    {
        Float32Msg message = new Float32Msg
        {
            data = reward
        };

        rosConnection.Publish(topicName,message);
    }
}

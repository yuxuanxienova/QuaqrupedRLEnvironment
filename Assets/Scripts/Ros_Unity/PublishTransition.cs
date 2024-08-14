using UnityEngine; 
using Unity.Robotics.ROSTCPConnector; 
using RosMessageTypes.Std;
using RosMessageTypes.RLTrainnerPkg;

public class PublishTransition : MonoBehaviour
{
    ROSConnection rosConnection;
    public string topicName="/unity/RL_Agent/transition";
    void Start()
    {
        rosConnection = ROSConnection.GetOrCreateInstance();
        rosConnection.RegisterPublisher<TransitionMsgMsg>(topicName);
        
    }

    // Update is called once per frame
    void Update()
    {

        
    }
    public void  CallPublishTransition(float[] state_tminus1, float[] action_tminus1, float[] reward_tminus1, float[] state_t, bool trancated_flag )
    {
        TransitionMsgMsg message = new TransitionMsgMsg
        {
            state = state_tminus1,
            action = action_tminus1,
            reward = reward_tminus1,
            next_state = state_t,
            trancated_flag = trancated_flag,
            layout = new RosMessageTypes.Std.MultiArrayLayoutMsg()
        };
        // Await the PublishAsync method to complete asynchronously
        rosConnection.Publish(topicName,message);
    }
}

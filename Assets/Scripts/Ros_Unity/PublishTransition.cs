using UnityEngine; 
using Unity.Robotics.ROSTCPConnector; 
using RosMessageTypes.Std;
using RosMessageTypes.RLTrainnerPkg;

public class PublishTransition : MonoBehaviour
{
    ROSConnection rosConnection;
    public string topicName="/unity/RL_Agent/transition";
    public Agent agent;

    public float publishMessageInterval = 0.5f;
    private float timeElapsed;
    
    private float[] state_stored;
    private float[] action_stored;
    private float[] reward_stored;
    void Start()
    {
        rosConnection = ROSConnection.GetOrCreateInstance();
        rosConnection.RegisterPublisher<TransitionMsgMsg>(topicName);
        
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > publishMessageInterval)
        {

            
            float[] state_tminus1 = state_stored;
            float[] action_tminus1 = action_stored;
            float[] reward_tminus1 = reward_stored;
            float[] state_t = agent.GetObservation();

            state_stored = agent.GetObservation();
            action_stored = agent.GetAction();
            reward_stored = agent.CalculateReward();

            TransitionMsgMsg message = new TransitionMsgMsg
            {
                state = state_tminus1,
                action = action_tminus1,
                reward = reward_tminus1,
                next_state = state_t,
                layout = new RosMessageTypes.Std.MultiArrayLayoutMsg()
            };

            //Publish Message
            rosConnection.Publish(topicName,message);
            timeElapsed = 0;
        }
        
    }
}

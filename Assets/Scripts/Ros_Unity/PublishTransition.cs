using UnityEngine; 
using Unity.Robotics.ROSTCPConnector; 
using RosMessageTypes.Std;
using RosMessageTypes.RLTrainnerPkg;

public class PublishTransition : MonoBehaviour
{
    ROSConnection rosConnection;
    public string topicName="/unity/RL_Agent/transition";
    public Agent agent;

    public float publishMessageFrequency = 0.5f;
    private float timeElapsed;
    // Start is called before the first frame update
    void Start()
    {
        rosConnection = ROSConnection.GetOrCreateInstance();
        rosConnection.RegisterPublisher<TransitionMsgMsg>(topicName);
        
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > publishMessageFrequency)
        {
            float[] state = new float[]{1.0f,2.0f};
            float[] action = new float[]{1.0f,2.0f};
            float[] reward = new float[]{1.0f,2.0f};
            float[] nextState = new float[]{1.0f,2.0f};

            TransitionMsgMsg message = new TransitionMsgMsg
            {
                state = state,
                action = action,
                reward = reward,
                next_state = nextState,
                layout = new RosMessageTypes.Std.MultiArrayLayoutMsg()
            };

            //Publish Message
            rosConnection.Publish(topicName,message);
            timeElapsed = 0;
        }
        
    }
}

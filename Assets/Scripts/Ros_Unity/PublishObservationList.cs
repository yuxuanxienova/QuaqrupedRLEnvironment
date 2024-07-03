using UnityEngine; 
using Unity.Robotics.ROSTCPConnector; 
using RosMessageTypes.Std;

public class PublishObservationList : MonoBehaviour
{
    ROSConnection rosConnection;
    public string topicName="/unity/RL_Agent/observationsList";
    public Agent agent;

    public float publishMessageFrequency = 0.5f;
    private float timeElapsed;
    // Start is called before the first frame update
    void Start()
    {
        rosConnection = ROSConnection.GetOrCreateInstance();
        rosConnection.RegisterPublisher<Float32MultiArrayMsg>(topicName);
        
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > publishMessageFrequency)
        {
            //Create the Message
            Float32MultiArrayMsg message = new Float32MultiArrayMsg();
            message.data = agent.GetObservation().ToArray();
            //Publish Message
            rosConnection.Publish(topicName,message);
            timeElapsed = 0;
        }
        
    }
}

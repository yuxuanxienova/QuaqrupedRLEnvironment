using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using System.Threading.Tasks;
using RosMessageTypes.RLTrainnerPkg;
using RosMessageTypes.Std;
public class ServiceClientSampleAction : MonoBehaviour
{
    public Agent agent;
    ROSConnection rosConnection;
    public string serviceName = "/trainner_node/service/sample_action";
    // Start is called before the first frame update
    void Start()
    {
        rosConnection = ROSConnection.GetOrCreateInstance();
        rosConnection.RegisterRosService<ProcessArrayRequest,ProcessArrayResponse>(serviceName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task<float[]> SampleActionFromObservationAsync(float[] obsArray)
    {
        return await CallSampleActionService(obsArray);
    }

    private Task<float[]> CallSampleActionService(float[] inputArray)
    {
        var tcs = new TaskCompletionSource<float[]>();

        ProcessArrayRequest request = new ProcessArrayRequest
        {
            input = new Float32MultiArrayMsg
            {
                data = inputArray
            }
        };

        rosConnection.SendServiceMessage<ProcessArrayResponse>(serviceName, request,
            (response) =>
            {
                if (response != null && response.output != null && response.output.data != null)
                {
                    tcs.SetResult(response.output.data);
                }
                else
                {
                    tcs.SetException(new System.Exception("Failed to get valid response"));
                }
            });

        return tcs.Task;
    }
}

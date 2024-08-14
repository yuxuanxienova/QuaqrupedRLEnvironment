using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using System.Threading.Tasks;
using RosMessageTypes.RLTrainnerPkg;
using RosMessageTypes.Std;
using System.Collections.Generic;
using System;
public class ServiceClientSampleAction : MonoBehaviour
{
    ROSConnection rosConnection;
    public string serviceNamehead = "/trainner_node/service/sample_action/id_";
    private string serviceName;
    private Dictionary<Guid, Action<float[]>> pendingRequests = new Dictionary<Guid, Action<float[]>>();
    // Start is called before the first frame update
    void Start()
    {
        serviceName = serviceNamehead + this.gameObject.GetComponent<Agent>().id;
        rosConnection = ROSConnection.GetOrCreateInstance();
        rosConnection.RegisterRosService<ProcessArrayRequest,ProcessArrayResponse>(serviceName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SampleActionFromObservationServiceRequest(float[] obsArray, Action<float[]> onResponse)
    {
        // Generate a unique identifier for the request
        Guid requestId = Guid.NewGuid();

        // Store the callback associated with this request
        pendingRequests[requestId] = onResponse;

        // Create the service request
        ProcessArrayRequest request = new ProcessArrayRequest
        {
            input = new Float32MultiArrayMsg
            {
                data = obsArray
            }
        };

        // Send the service request and pass the request ID to the response handler
        rosConnection.SendServiceMessage<ProcessArrayResponse>(serviceName, request, response => OnServiceResponse(response, requestId));
    }

    private void OnServiceResponse(ProcessArrayResponse response, Guid requestId)
    {
        if (pendingRequests.TryGetValue(requestId, out var onResponse))
        {
            // Remove the request from the dictionary
            pendingRequests.Remove(requestId);

            // Pass the response data to the corresponding agent's callback
            onResponse?.Invoke(response.output.data);
        }
        else
        {
            // Handle the case where the request ID is not found
            Debug.LogError("Received response for unknown request ID");
        }
    }
}

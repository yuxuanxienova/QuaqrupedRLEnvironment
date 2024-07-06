using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class UIController : MonoBehaviour
{
    public Agent agent;
    public ArticulationBody articulationBody;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ResetClicked()
    {
        agent.Reset();
    }
    public void TestGetObservationCLicked()
    {
        string floatListToString = ExtensionMethods.FloatArrayToString(agent.GetObservation());
        Debug.Log("[INFO][UIController][TestGetObservationCLicked]ObservationList:"+floatListToString);
    }

    public void TestActionClicked()
    {
        articulationBody.SetDriveStiffness(ArticulationDriveAxis.X,30f);
        articulationBody.SetDriveTarget(ArticulationDriveAxis.X,5f);

    }
    public void TestGetActionClicked()
    {
        string floatListToString = ExtensionMethods.FloatArrayToString(agent.GetAction());
        Debug.Log("[INFO][UIController][TestGetActionClicked]ActionList:"+floatListToString);
    }

    public async void TestSampleActionAsyncClicked()
    {
        float[] action_arr = await agent.SampleActionAsync(agent.GetObservation());
        Debug.Log("[INFO][UIController][TestSampleActionAsyncClicked]action_arr:"+ExtensionMethods.FloatArrayToString(action_arr));
    }
}

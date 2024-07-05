using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

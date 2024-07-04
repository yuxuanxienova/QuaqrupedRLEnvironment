using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumAgentController : AgentControllerBase
{
    public Transform joint_prismatic;
    public Transform joint_revolute;

    private ArticulationBody articulationBody_joint_prismatic;
    private ArticulationBody articulationBody_joint_revolute;

    void Start()
    {
        articulationBody_joint_prismatic = joint_prismatic.GetComponent<ArticulationBody>();
        articulationBody_joint_revolute = joint_revolute.GetComponent<ArticulationBody>();
    }
    public override void SetAction(List<float> _action_list)
    {
        //Action Space is 4
        articulationBody_joint_prismatic.SetDriveStiffness(ArticulationDriveAxis.X,_action_list[0]);
        articulationBody_joint_prismatic.SetDriveTarget(ArticulationDriveAxis.X,_action_list[1]);

        articulationBody_joint_revolute.SetDriveStiffness(ArticulationDriveAxis.X,_action_list[2]);
        articulationBody_joint_revolute.SetDriveTarget(ArticulationDriveAxis.X,_action_list[3]);

        //Store the action list
        action_list =_action_list;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

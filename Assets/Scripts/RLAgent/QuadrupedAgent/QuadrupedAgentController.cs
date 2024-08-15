using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadrupedAgentController : AgentControllerBase
{
    public GameObject basePrefab;
    private Vector3 agent_init_position;
    private Transform base_inertia;
    private Transform root;
    private Transform RH_HIP;
    private Transform RH_THIGH;
    private Transform RH_SHANK;
    private Transform RH_FOOT;
    private Transform LH_HIP;
    private Transform LH_THIGH;
    private Transform LH_SHANK;
    private Transform LH_FOOT;
    private Transform RF_HIP;
    private Transform RF_THIGH;
    private Transform RF_SHANK;
    private Transform RF_FOOT;
    private Transform LF_HIP;
    private Transform LF_THIGH;
    private Transform LF_SHANK;
    private Transform LF_FOOT;

    private GroundContactCustom groundContact_RH_THIGH;
    private GroundContactCustom groundContact_RH_SHANK1;
    private GroundContactCustom groundContact_RH_SHANK2;
    private GroundContactCustom groundContact_RH_FOOT;

    private GroundContactCustom groundContact_LH_THIGH;
    private GroundContactCustom groundContact_LH_SHANK1;
    private GroundContactCustom groundContact_LH_SHANK2;
    private GroundContactCustom groundContact_LH_FOOT;

    private GroundContactCustom groundContact_RF_THIGH;
    private GroundContactCustom groundContact_RF_SHANK1;
    private GroundContactCustom groundContact_RF_SHANK2;
    private GroundContactCustom groundContact_RF_FOOT;

    private GroundContactCustom groundContact_LF_THIGH;
    private GroundContactCustom groundContact_LF_SHANK1;
    private GroundContactCustom groundContact_LF_SHANK2;
    private GroundContactCustom groundContact_LF_FOOT;

    //This will be used as a stabilized model space reference point for observations
    //Because ragdolls can move erratically during training, using a stabilized reference transform improves learning
    // public OrientationCubeController m_OrientationCube;

    //The indicator graphic gameobject that points towards the target
    // public DirectionIndicator m_DirectionIndicator;

    private ArticulationBody articulationBody_Root;
    private ArticulationBody articulationBody_BaseInertia;

    private ArticulationBody articulationBody_RH_HIP;
    private ArticulationBody articulationBody_RH_THIGH;
    private ArticulationBody articulationBody_RH_SHANK;

    private ArticulationBody articulationBody_LH_HIP;
    private ArticulationBody articulationBody_LH_THIGH;
    private ArticulationBody articulationBody_LH_SHANK;

    private ArticulationBody articulationBody_RF_HIP;
    private ArticulationBody articulationBody_RF_THIGH;
    private ArticulationBody articulationBody_RF_SHANK;

    private ArticulationBody articulationBody_LF_HIP;
    private ArticulationBody articulationBody_LF_THIGH;
    private ArticulationBody articulationBody_LF_SHANK;
    public override void ExecuteAction()
    {
        throw new System.NotImplementedException();
    }

    public override void Reset()
    {
        Destroy(root.gameObject);
        InitializeBodyParts();
    }

    // Start is called before the first frame update
    void Start()
    {
        agent_init_position = transform.position;
        InitializeBodyParts();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool BodyTouchingGround()
    { 
        bool touchingGround = groundContact_LF_SHANK1.touchingGround || groundContact_LF_SHANK2.touchingGround || groundContact_LF_THIGH.touchingGround ||
        groundContact_LH_SHANK1.touchingGround || groundContact_LH_SHANK2.touchingGround || groundContact_LH_THIGH.touchingGround ||
        groundContact_RF_SHANK1.touchingGround || groundContact_RF_SHANK2.touchingGround || groundContact_RF_THIGH.touchingGround ||
        groundContact_RH_SHANK1.touchingGround || groundContact_RH_SHANK2.touchingGround || groundContact_RH_THIGH.touchingGround;  
        return touchingGround;
    }
    public void InitializeBodyParts()
    {
        GameObject newBase =  Instantiate(basePrefab, agent_init_position, Quaternion.identity);
        newBase.transform.SetParent(transform, false);
        root = newBase.transform;

        //Get all needed Transform
        Transform[] allChildren = newBase.GetComponentsInChildren<Transform>();

        foreach (Transform child_tf in allChildren)
        {
            Debug.Log(child_tf.name);
            if (child_tf.name == "base_inertia")
            {
                base_inertia = child_tf;                
            }

            if (child_tf.name == "RH_HIP")
            {
                RH_HIP = child_tf;                
            }         
            if (child_tf.name == "RH_THIGH")
            {
                RH_THIGH = child_tf;                
            }   
            if (child_tf.name == "RH_SHANK")
            {
                RH_SHANK = child_tf;                
            }  
            if (child_tf.name == "RH_FOOT")
            {
                RH_FOOT = child_tf;                
            }  


            if (child_tf.name == "RF_HIP")
            {
                RF_HIP = child_tf;                
            }         
            if (child_tf.name == "RF_THIGH")
            {
                RF_THIGH = child_tf;                
            }   
            if (child_tf.name == "RF_SHANK")
            {
                RF_SHANK = child_tf;                
            }  
            if (child_tf.name == "RF_FOOT")
            {
                RF_FOOT = child_tf;                
            }   


            if (child_tf.name == "LH_HIP")
            {
                LH_HIP = child_tf;                
            }         
            if (child_tf.name == "LH_THIGH")
            {
                LH_THIGH = child_tf;                
            }   
            if (child_tf.name == "LH_SHANK")
            {
                LH_SHANK = child_tf;                
            }  
            if (child_tf.name == "LH_FOOT")
            {
                LH_FOOT = child_tf;                
            }  


            if (child_tf.name == "LF_HIP")
            {
                LF_HIP = child_tf;                
            }         
            if (child_tf.name == "LF_THIGH")
            {
                LF_THIGH = child_tf;                
            }   
            if (child_tf.name == "LF_SHANK")
            {
                LF_SHANK = child_tf;                
            }  
            if (child_tf.name == "LF_FOOT")
            {
                LF_FOOT = child_tf;                
            }  

            if (child_tf.name == "ContactSensor_RH_THIGH")
            {
                groundContact_RH_THIGH = child_tf.gameObject.GetComponent<GroundContactCustom>();                
            }
            if (child_tf.name == "ContactSensor_RH_SHANK1")
            {
                groundContact_RH_SHANK1 = child_tf.gameObject.GetComponent<GroundContactCustom>();                
            }
            if (child_tf.name == "ContactSensor_RH_SHANK2")
            {
                groundContact_RH_SHANK2 = child_tf.gameObject.GetComponent<GroundContactCustom>();                
            }
            if (child_tf.name == "ContactSensor_RH_FOOT")
            {
                groundContact_RH_FOOT = child_tf.gameObject.GetComponent<GroundContactCustom>();                
            }

            if (child_tf.name == "ContactSensor_RF_THIGH")
            {
                groundContact_RF_THIGH = child_tf.gameObject.GetComponent<GroundContactCustom>();                
            }
            if (child_tf.name == "ContactSensor_RF_SHANK1")
            {
                groundContact_RF_SHANK1 = child_tf.gameObject.GetComponent<GroundContactCustom>();                
            }
            if (child_tf.name == "ContactSensor_RF_SHANK2")
            {
                groundContact_RF_SHANK2 = child_tf.gameObject.GetComponent<GroundContactCustom>();                
            }
            if (child_tf.name == "ContactSensor_RF_FOOT")
            {
                groundContact_RF_FOOT = child_tf.gameObject.GetComponent<GroundContactCustom>();                
            }  

            if (child_tf.name == "ContactSensor_LH_THIGH")
            {
                groundContact_LH_THIGH = child_tf.gameObject.GetComponent<GroundContactCustom>();                
            }
            if (child_tf.name == "ContactSensor_LH_SHANK1")
            {
                groundContact_LH_SHANK1 = child_tf.gameObject.GetComponent<GroundContactCustom>();                
            }
            if (child_tf.name == "ContactSensor_LH_SHANK2")
            {
                groundContact_LH_SHANK2 = child_tf.gameObject.GetComponent<GroundContactCustom>();                
            }
            if (child_tf.name == "ContactSensor_LH_FOOT")
            {
                groundContact_LH_FOOT = child_tf.gameObject.GetComponent<GroundContactCustom>();                
            }  

             if (child_tf.name == "ContactSensor_LF_THIGH")
            {
                groundContact_LF_THIGH = child_tf.gameObject.GetComponent<GroundContactCustom>();                
            }
            if (child_tf.name == "ContactSensor_LF_SHANK1")
            {
                groundContact_LF_SHANK1 = child_tf.gameObject.GetComponent<GroundContactCustom>();                
            }
            if (child_tf.name == "ContactSensor_LF_SHANK2")
            {
                groundContact_LF_SHANK2 = child_tf.gameObject.GetComponent<GroundContactCustom>();                
            }
            if (child_tf.name == "ContactSensor_LF_FOOT")
            {
                groundContact_LF_FOOT = child_tf.gameObject.GetComponent<GroundContactCustom>();                
            }  
        }
    }
}

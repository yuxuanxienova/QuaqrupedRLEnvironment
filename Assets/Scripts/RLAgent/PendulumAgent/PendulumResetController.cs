using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumResetController : MonoBehaviour
{
    public float resetInterval=10;
    private float timeElapsed;

    public Agent agent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > resetInterval)
        {
            agent.Reset();

        }
    }
}

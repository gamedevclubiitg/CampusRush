using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    float acceleration = 3, retardation, finalVelocity, finalVelocityback, angularVelocity, angularVelocitystop, finalAngularVel, finalAngularVelstop;
    [SerializeField]
    Vector3 velocity, com;
    [SerializeField]
    float time = 0;
    float velocity1, velocity2;
    Rigidbody rb;
    //bool once = false;
    // Start is called before the first frame update
    void Awake()
    {
        velocity = Vector3.zero;
        rb = gameObject.GetComponent<Rigidbody>();
        //com = new Vector3(3.09f, 0.3848695f, 3.29f);
    }

    // Update is called once per frame
    void Update()
    {
        //rb.centerOfMass = com;
        //velocity1 = rb.velocity.magnitude;
        //velocity2 = GameObject.Find("Backside").GetComponent<Rigidbody>().velocity.magnitude;
        //if(velocity1 < velocity2)
        {
            //time += Time.deltaTime;
        }
        //Debug.Log(this.transform.forward);
        if (Input.GetAxis("Vertical") > 0)
        {
            if (rb.velocity.magnitude < finalVelocity)
            {
                rb.AddForce(this.transform.forward * acceleration, ForceMode.Acceleration);
            }
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            if (rb.velocity.magnitude < finalVelocityback)
            {
                rb.AddForce(this.transform.forward * retardation, ForceMode.Acceleration);
            }
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            if(rb.velocity.magnitude < 0.5 && rb.angularVelocity.magnitude < finalAngularVelstop)
            {
                Debug.Log(rb.velocity.magnitude+" "+rb.angularVelocity.magnitude);
                rb.AddTorque(0, Time.deltaTime * angularVelocitystop * Input.GetAxis("Horizontal"), 0, ForceMode.Acceleration);
            }
            else if (rb.angularVelocity.magnitude < finalAngularVel)
            {
                //Debug.Log(angularVelocity * Time.deltaTime * Input.GetAxis("Horizontal"));
                rb.AddTorque(0, Time.deltaTime * angularVelocity * Input.GetAxis("Horizontal"), 0, ForceMode.Acceleration);
                
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(gameObject.GetComponent<Rigidbody>().centerOfMass, 0.5f);
    }
}

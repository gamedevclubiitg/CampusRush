using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Basketball : MonoBehaviour
{
    PlayerBasketball Pb;

    private float holddownstarttime;
    public float maxforceholdtime = 1f;
    public float minForce = 3f;
    public float maxForce = 15f;
    private bool tt=false;
    public Slider powerLevel;
    void Start()
    {
       powerLevel.minValue=0f;
       powerLevel.maxValue=maxforceholdtime;
       powerLevel.value=0f;
       Pb = GetComponent<PlayerBasketball>();
    }

    
    void Update()
    {
        if (Pb.holdingball)
        {
            Pb.Ball.transform.position = Pb.ballpos.transform.position;
                // powerLevel.value=0f;
            if (Input.GetMouseButtonDown(0))
            {
                
                holddownstarttime = Time.time;
               
                
                tt=true;
            }
            // Debug.Log(holddownstarttime);
            if(tt){
            powerLevel.value=Time.time - holddownstarttime;

            }
            
            // Debug.Log(powerLevel.value);
            if (Input.GetMouseButtonUp(0))
            {
                float holddowntime = Time.time - holddownstarttime;
               
                if (holddowntime < 0.2f)
                {
                    Pb.launchball(minForce);
                    powerLevel.value=0;
                }
                else
                {
                    Pb.launchball(calholdforce(holddowntime));
                    powerLevel.value=0;
                }
                tt=false;
            }
        }
        

        if (!Pb.holdingball)
        {
            if (Input.GetKey(KeyCode.E))
            {
                Pb.stopball();
            }
        }
    }

    private float calholdforce(float holdtime)
    {
        float holdtimenormalize = Mathf.Clamp01(holdtime / maxforceholdtime);
        float force = holdtimenormalize * maxForce;
        return force;
    }


}

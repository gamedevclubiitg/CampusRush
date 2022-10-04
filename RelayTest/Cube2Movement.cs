using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Cube2Movement : NetworkBehaviour
{
    [SerializeField]
    private float speed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        if(IsLocalPlayer)
        {
            transform.position = new Vector3(UnityEngine.Random.Range(-10,10),2,UnityEngine.Random.Range(-10,10));
        }
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 dir = new Vector3(horizontal,0,vertical).normalized;

        gameObject.GetComponent<Rigidbody>().AddForce(dir*speed, ForceMode.VelocityChange);
    }
}

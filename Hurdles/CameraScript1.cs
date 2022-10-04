using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript1 : MonoBehaviour
{
    public float mousesensi = 20f;
    float xrotation = 0f;
    public Transform playerbody;

    
    void Start()
    {
        //xrotation = transform.rotation.eulerAngles.y;
    }

    
    void Update()
    {
        float MouseX = Input.GetAxisRaw("Mouse X") * mousesensi * Time.deltaTime;
        float MouseY = Input.GetAxisRaw("Mouse Y") * mousesensi * Time.deltaTime;
        
        xrotation -= MouseY;
        xrotation = Mathf.Clamp(xrotation, -50f, 50f);

        transform.localRotation = Quaternion.Euler(xrotation, 0f , 0f);
        playerbody.Rotate(Vector3.up * MouseX);
    }
}

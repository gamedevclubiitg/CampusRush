using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float mousesensi = 20f;
    float xrotation = 0f;

    
    void Start()
    {
        //xrotation = transform.rotation.eulerAngles.y;
    }

    
    void Update()
    {
        float MouseX = Input.GetAxisRaw("Mouse X") * mousesensi * Time.deltaTime;
        float MouseY = Input.GetAxisRaw("Mouse Y") * mousesensi * Time.deltaTime;
        
        xrotation += MouseX;

        transform.localRotation = Quaternion.Euler(0f, xrotation, 0f);
    }
}

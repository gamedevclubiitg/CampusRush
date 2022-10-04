using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skyboxrender : MonoBehaviour
{
    public Material skyBox;
    void Awake()
    {
        RenderSettings.skybox=skyBox;
    }
    
}
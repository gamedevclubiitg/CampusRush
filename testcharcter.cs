using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testcharcter : MonoBehaviour
{
  

    [SerializeField]
    Dropdown dp;


    void Start()
    {

    }


    void Update()
    {
        dp.RefreshShownValue();
        
    }

    public void testonchange()
    {
        Debug.Log("character changed");
        Debug.Log(dp.value);
    }
}

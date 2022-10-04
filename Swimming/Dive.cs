using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dive : MonoBehaviour
{
    public Text count;

    // Update is called once per frame
    void Update()
    {
        Debug.Log(count.text);
        if(count.text=="GO!"){
            gameObject.GetComponent<PlayerSwimAI>().Dive();

        }
        
    }
}
